/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.HandController;
using Libdexmo.Unity.Core.Utility;

namespace Libdexmo.Unity.HandController
{
    public abstract class HandGestureRecognizerBase: MonoBehaviour, IHandGestureRecognizer
    {
        public event EventHandler<GenericEventArgs<bool>> SwitchHandCollisionDetection;
        public event EventHandler<GenericEventArgs<bool>> SwitchHandMotionTransition;

        private UnityHandPoseManager _handPoseManager;
        protected HandPoseType HandPose { get; private set; }

        public void AttachHandPoseManager(IHandPoseManager handPoseManager)
        {
            _handPoseManager = handPoseManager as UnityHandPoseManager;
            if (_handPoseManager != null)
            {
                HandPose = _handPoseManager.Pose;
                _handPoseManager.HandPoseChanged += HandPoseChangedHandler;
            }
        }

        protected virtual void HandPoseChangedHandler(object sender, HandPoseChangedEventArgs args)
        {
            HandPose = args.Pose;
        }

        protected void OnSwitchHandCollisionDetection(bool enableCollision)
        {
            GenericEventArgs<bool> args = new GenericEventArgs<bool>(enableCollision);
            Miscellaneous.InvokeEvent(SwitchHandCollisionDetection, this, args);
        }

        protected void OnSwitchHandMotionTransition(bool enableMotionTransition)
        {
            GenericEventArgs<bool> args = new GenericEventArgs<bool>(enableMotionTransition);
            Miscellaneous.InvokeEvent(SwitchHandMotionTransition, this, args);
        }

        protected virtual void OnDestroy()
        {
            if (_handPoseManager != null)
            {
                _handPoseManager.HandPoseChanged -= HandPoseChangedHandler;
            }
        }
    }
}
