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
using Libdexmo.Unity.Pickers;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.HandController;
using UnityEngine;

namespace Libdexmo.Unity.Snapping
{
    /// <summary>
    /// This script gives snapper behaviour for hand models.
    /// </summary>
    public class SnapperDexmo: Snapper
    {
        /// <summary>
        /// Whether the associated hand model is right hand.
        /// </summary>
        public bool IsRight { get; private set; }
        /// <summary>
        /// Most parent transform of the hand model.
        /// </summary>
        public Transform HandRootTransform { get { return _handRootTranform; } }
        /// <summary>
        /// This parameter has duplicate function as the snapping transition distance min
        /// of snappable objects. It will be removed in the future version.
        /// </summary>
        public bool HandMotionInTransitionSnapping
        { get { return _handMotionInTransitionSnapping; } }
        /// <summary>
        /// Transform of the palm center of the hand model.
        /// </summary>
        public Transform PalmCenter { get { return _palmCenter; } }

        private Transform _handRootTranform;
        private Transform _palmCenter;
        private IHandControllerSettings _handControllerSettings;

        [Tooltip("A value between 0 and 1. The greater the value is, the " +
                 "faster the position of the hand model will reach the target " +
                 "position during snapping.")]
        [SerializeField]
        private float _handPositionTransitionCoefficientSnapping = 0.8f;
        [Tooltip("A value between 0 and 1. The greater the value is, the faster " +
                 "the rotation of the hand model will reach the target rotation " +
                 "during snapping")]
        [SerializeField]
        private float _handRotationTransitionCoefficientSnapping = 0.8f;

        [Tooltip("Deprecated. Will be removed in the future")]
        [HideInInspector]
        [SerializeField]
        private float _handMotionTransitionDistanceMinSnapping = 0.01f;

        private bool _handMotionInTransitionSnapping;

        protected override void Init()
        {
            base.Init();
            _handMotionInTransitionSnapping = false;
        }

        void Awake()
        {
            Init();
        }

        /// <summary>
        /// Save references to variables for later use.
        /// </summary>
        /// <param name="isRight">Whether the hand model is right hand.</param>
        /// <param name="handRootTransform">Most parent transform of the hand model.</param>
        /// <param name="palmCenterTransform">Palm center transform of the hand model.</param>
        /// <param name="handControllerSettings">Hand controller settings that allow to
        /// be modified.</param>
        public void AttachHandController(bool isRight, Transform handRootTransform,
            Transform palmCenterTransform, IHandControllerSettings handControllerSettings)
        {
            _handRootTranform = handRootTransform;
            _palmCenter = palmCenterTransform;
            IsRight = isRight;
            _handControllerSettings = handControllerSettings;
        }

        /// <summary>
        /// Modify hand motion transition parameters. The snapping transition movement
        /// is controlled by UnityHandController in <see cref="DexmoController"/>.
        /// </summary>
        private void ModifyHandMotionTransitionParameters()
        {
            _handControllerSettings.HandPositionTransitionCoefficient =
                _handPositionTransitionCoefficientSnapping;
            _handControllerSettings.HandRotationTransitionCoefficient =
                _handRotationTransitionCoefficientSnapping;
            _handControllerSettings.HandMotionTransitionDistanceMin =
                _handMotionTransitionDistanceMinSnapping;
        }

        /// <summary>
        /// Restore the hand controller settings to its default values. When setting
        /// those parameters to -1, hand controllers wiil automatically set it
        /// back to default values.
        /// </summary>
        private void RestoreHandMotionTransitionParameters()
        {
            _handControllerSettings.HandPositionTransitionCoefficient = -1;
            _handControllerSettings.HandRotationTransitionCoefficient = -1;
            _handControllerSettings.HandMotionTransitionDistanceMin = -1;
        }

        /// <summary>
        /// Start snapping transition.
        /// </summary>
        private void StartTransition()
        {
            ModifyHandMotionTransitionParameters();
            _handMotionInTransitionSnapping = true;
            _handControllerSettings.DetectCollision = false;
        }

        /// <summary>
        /// Stop snapping transition.
        /// </summary>
        private void StopTransition()
        {
            RestoreHandMotionTransitionParameters();
            _handMotionInTransitionSnapping = false;
            _handControllerSettings.DetectCollision = true;
        }

        /// <summary>
        /// Stop snapping transition.
        /// </summary>
        public void StopHandMotionTransitionSnapping()
        {
            StopTransition();
        }

        /// <summary>
        /// Check if the snapper is still moving towards its target position.
        /// </summary>
        public void CheckHandMotionInTransition()
        {
            if (!IsSnapped)
            {
                return;
            }
            if (_handMotionInTransitionSnapping)
            {
                bool inSnappingTransition = SnappedObject.CheckInSnappingTransition(this);
                if (!inSnappingTransition)
                {
                    StopTransition();
                }
            }
        }

        /// <summary>
        /// This will be called when the snapper just enters the snapping region. It
        /// starts the snapping transition.
        /// </summary>
        protected override void OnSnappedEnter()
        {
            StartTransition();
            base.OnSnappedEnter();
            Debug.Log("On Snapped enter.");
        }

        /// <summary>
        /// This will be called when the snapper just exits the snapping region. It
        /// stops the snapping transition.
        /// </summary>
        protected override void OnSnappedExit()
        {
            StopTransition();
            base.OnSnappedExit();
            Debug.Log("On snapped exit.");
        }

        /// <summary>
        /// Constrain finger rotation of hand models while snapping according to the
        /// hand constraint settings of the snappable object if any.
        /// </summary>
        /// <remarks>
        /// Hand constraint is useful during snapping because it reduces the degree
        /// of freedoms of the hand model and makes it subsequent behaviour more
        /// predictable and easier to control.
        /// </remarks>
        /// <param name="handData">Hand rotation data that will be modified to
        /// constrain finger rotation.</param>
        public void ConstrainHandDataWhileSnapping(Hand handData)
        {
            if (!IsSnapped)
            {
                return;
            }
            SnappableCommonForDexmo snappedObject = SnappedObject as SnappableCommonForDexmo;
            if (snappedObject != null)
            {
                snappedObject.HandConstraint.ConstrainHandData(handData);
            }
        }
    }
}
