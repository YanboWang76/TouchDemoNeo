/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Model;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.HandController;
using Libdexmo.Unity.Core.Utility;
using UnityEngine;

namespace Libdexmo.Unity.HandController
{
    public sealed class UnityHandPoseManager : MonoBehaviour, IHandPoseManager
    {
        public event EventHandler<HandPoseChangedEventArgs> HandPoseChanged;

        public bool IsRight { get; private set; }
        public IHandRotationNormalized CurHandRotationNormalized
        {
            get
            {
                UpdateHandRotationNormalizedInfo();
                return _curHandRotationNormalized;
            }
        }
        public HandPoseType Pose { get; private set; }

        private IFingerRotationManager[] _fingers;
        private HandRotationNormalizedInfo _curHandRotationNormalized;
        private HandPoseConditionManager _handPoseConditionManager;

        public void AttachFingerRotationManager(bool isRight, IFingerRotationManager[] fingers)
        {
            IsRight = isRight;
            _curHandRotationNormalized = new HandRotationNormalizedInfo();
            int n = fingers.Length;
            if (n != 5)
            {
                Debug.LogError("Finger length is incorrect.");
                return;
            }
            _fingers = new IFingerRotationManager[n];
            for (int i = 0; i < n; i++)
            {
                _fingers[i] = fingers[i];
            }
            _handPoseConditionManager = HandPoseConditionManager.Instance;
            Pose = HandPoseType.Normal;
        }

        private void OnPoseChanged()
        {
            HandPoseChangedEventArgs args = new HandPoseChangedEventArgs(Pose);
            Miscellaneous.InvokeEvent(HandPoseChanged, this, args);
        }

        public void UpdateHandPose(Hand handData)
        {
            UpdateHandPoseWorker(handData);
        }

        private void UpdateHandPoseWorker(Hand handData)
        {
            if (_handPoseConditionManager == null)
            {
                return;
            }
            HandPoseType lastPose = Pose;
            switch (lastPose)
            {
                case HandPoseType.Normal:
                    if (_handPoseConditionManager.CheckSatisfyCondition(
                        HandPoseType.SpidermanReady, handData))
                    {
                        Pose = HandPoseType.SpidermanReady;
                    }
                    break;

                case HandPoseType.SpidermanReady:
                    if (_handPoseConditionManager.CheckSatisfyCondition(
                        HandPoseType.SpidermanShoot, handData))
                    {
                        Pose = HandPoseType.SpidermanShoot;
                    }
                    else if (_handPoseConditionManager.CheckSatisfyCondition(
                            HandPoseType.SpidermanReset, handData))
                    {
                        Pose = HandPoseType.SpidermanReset;
                    }
                    break;

                case HandPoseType.SpidermanShoot:
                    Pose = HandPoseType.Normal;
                    break;

                case HandPoseType.SpidermanReset:
                    Pose = HandPoseType.Normal;
                    break;
            }

            if (lastPose != Pose)
            {
                OnPoseChanged();
            }
        }

        private void UpdateHandRotationNormalizedInfo()
        {
            int n = _fingers.Length;
            for (int i = 0; i < n; i++)
            {
                IFingerRotationManager finger = _fingers[i];
                FingerType fingerType = finger.FingerType;
                float[] fingerRotation = finger.GetCurrentJointRotation();
                _curHandRotationNormalized.UpdateFingerRotationNormalizedInfo(
                    fingerType, fingerRotation);
            }
        }
    }
}
