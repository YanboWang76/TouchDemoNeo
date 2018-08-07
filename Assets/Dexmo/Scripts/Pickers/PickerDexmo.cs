/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Libdexmo.Model;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Model;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Snapping;
using Libdexmo.Unity.Triggering;

namespace Libdexmo.Unity.Pickers
{
    /// <summary>
    /// This script must be attached to every hand models controlled by Dexmo. It implements
    /// <see cref="IPicker"/> interface to interact with pickable objects. If force feedback
    /// is needed, the object grasped must attach script that implements 
    /// <see cref="IPickableForceFeedback"/> interface.
    /// <remarks>
    /// Since snapping interaction is closely related to picking, snapper object is a member
    /// of this script.
    /// </remarks>
    /// </summary>
    public sealed class PickerDexmo : Picker, IPickerDexmo
    {
        /// <summary>
        /// This is the palm centre of the hand model. It should be roughly the centre of
        /// the palm when hand is grasping other object.
        /// </summary>
        public Transform PalmCenter { get; private set; }
        /// <summary>
        /// This is the most parent transform of the hand model, so moving this transform
        /// will move the entire hand model.
        /// </summary>
        public Transform HandRootTransform { get { return _handRootTransform; } }

        private IFingerTriggerColliderManager[] _fingerTriggers;
        // This manager will collect collisions between picking prediction indicator
        // game object and touchable objects and change touchable objects color if needed.
        // See "Picking Prediction Indicator" in Libdexmo Unity SDK docs for more details.
        private TriggeringTouchableManager _pickingPredictionIndicatorManager;
        private HashSet<ITouchable> _touchableSet;
        private bool _initialized;
        private Transform _handRootTransform;
        private SnapperDexmo _snapper;
        private PickableColorModifier _colorModifier;
        private bool isRight;

        protected override void Init()
        {
            base.Init();
            _fingerTriggers = new IFingerTriggerColliderManager[5];
            _touchableSet = new HashSet<ITouchable>();
            _colorModifier = new PickableColorModifier();
            _initialized = false;
        }

        /// <summary>
        /// It saves the reference to objects passed as arguments for later use.
        /// </summary>
        /// <param name="isRight">Whether this picker is for right hand.</param>
        /// <param name="triggerColliderModel">The trigger collider hand model.</param>
        /// <param name="palmCenterTransform">Transform of the palm center.</param>
        /// <param name="handControllerSettings">Hand controller settings that allow modification.</param>
        public void AttachHandController(bool isRight, UnityHandModel triggerColliderModel,
            Transform palmCenterTransform, IHandControllerSettings handControllerSettings)
        {
            if (triggerColliderModel == null)
            {
                Debug.LogError("Trigger collider model is null");
                return;
            }
            int n = triggerColliderModel.Fingers.Length;
            for (int i = 0; i < n; i++)
            {
                UnityFingerModel fingerModel = triggerColliderModel.Fingers[i];
                IFingerTriggerColliderManager manager =
                    fingerModel.Finger.GetComponent<IFingerTriggerColliderManager>();
                if (manager == null)
                {
                    Debug.LogErrorFormat("Unable to find trigger collider manager on finger {0}", i);
                }
                _fingerTriggers[i] = manager;
            }
            PalmCenter = palmCenterTransform;
            _handRootTransform = triggerColliderModel.HandRootTransform;
            _initialized = true;
            FindSnapper(isRight, _handRootTransform, palmCenterTransform, handControllerSettings);
            FindTriggeringTouchableManager(triggerColliderModel.Hand);
        }

        /// <summary>
        /// Find the <see cref="TriggeringTouchableManager"/> of the hand model, which will
        /// be used to collect collision only with touchable objects.
        /// </summary>
        /// <param name="hand">Hand transform to search from.</param>
        private void FindTriggeringTouchableManager(Transform hand)
        {
            TriggeringTouchableManager manager = null;
            foreach (Transform child in hand)
            {
                manager = child.GetComponent<TriggeringTouchableManager>();
                if (manager != null)
                {
                    break;
                }
            }
            _pickingPredictionIndicatorManager = manager;
        }

        /// <summary>
        /// Find snapper object from hand transform. Snapper gives snapping behaviour
        /// of its hand model. It lets hand model to snap onto other objects attached with
        /// snappable scripts.
        /// </summary>
        /// <param name="isRight">Whether the hand model is right hand.</param>
        /// <param name="handRootTransform">Hand root transform of the hand model</param>
        /// <param name="palmCenterTransform">Palm centre transform</param>
        /// <param name="handControllerSettings">Public hand controller settings
        /// that may be modified by snapper.</param>
        private void FindSnapper(bool isRight, Transform handRootTransform, 
            Transform palmCenterTransform,
            IHandControllerSettings handControllerSettings)
        {
            _snapper = _handRootTransform.GetComponentInChildren<SnapperDexmo>();
            if (_snapper == null)
            {
                Debug.LogWarning("Unable to find snapperDexmo.");
                return;
            }
            // Save some variables in snapper for later use.
            _snapper.AttachHandController(isRight, handRootTransform, 
                palmCenterTransform, handControllerSettings);
        }

        /// <summary>
        /// Try picking up the object. Return true if successful.
        /// </summary>
        /// <param name="pickable">Object to be picked up.</param>
        /// <returns>True if the pick-up is successful.</returns>
        public bool TryPicking(IPickableForceFeedback pickable)
        {
            if (_snapper != null && _snapper.HandMotionInTransitionSnapping)
            {
                // Wait until snapping finishes. Snapping will move the hand model
                // gradually to the target snapping position. Before it reaches the
                // target position, it should not start to grasp anything.
                return false;
            }
            bool success = AttachPickable(pickable);
            return success;
        }

        /// <summary>
        /// Release the pickable object held.
        /// </summary>
        public void Release()
        {
            StopPicking();
        }

        /// <summary>
        /// This function will be called at every FixedUpdate cycle. It will update the picking
        /// interaction as well as snapping interaction.
        /// </summary>
        public void PickerFixedUpdate()
        {
            if (_snapper != null)
            {
                if (!IsHolding)
                {
                    // Only update snapper when it is not holding anything.
                    _snapper.SnapUpdate();
                }
                else
                {
                    if (_snapper.HandMotionInTransitionSnapping)
                    {
                        _snapper.StopHandMotionTransitionSnapping();
                    }
                }
            }
            PickingFixedUpdate();
        }

        /// <summary>
        /// This function calls snapper's function to constrain finger rotation while
        /// rotation.
        /// </summary>
        /// <remarks>
        /// During snapping, it is allowed to constrain hand model's finger rotation
        /// to certain range regardless of finger tracking in reality. Limiting degree of
        /// freedoms is useful because subsequent grasping behaviour of hand model will be
        /// more predictable. More details can be found in "Snappable" section of Libdexmo
        /// Unity SDK docs.
        /// </remarks>
        /// <param name="handData">Hand tracking data that will be modified to give 
        /// constrained finger rotation data.</param>
        public void ConstrainHandWhileSnapping(Hand handData)
        {
            if (_snapper != null)
            {
                _snapper.ConstrainHandDataWhileSnapping(handData);
            }
        }

        /// <summary>
        /// This function calls the snapper's function to update check if hand model is still
        /// moving towards the snapping target position.
        /// </summary>
        public void UpdateSnappingTransitionStatus()
        {
            if (_snapper != null)
            {
                _snapper.CheckHandMotionInTransition();
            }
        }

        /// <summary>
        /// Update touched object's color in LateUpdate, after all the collision detection
        /// has finished.
        /// </summary>
        void LateUpdate()
        {
            if (!_initialized)
            {
                return;
            }
            UpdateTouchedObjectColor();
        }

        #region Modify color of touched object
        /// <summary>
        /// Update touched object's color if needed.
        /// </summary>
        private void UpdateTouchedObjectColor()
        {
            _touchableSet.Clear();
            int n = _fingerTriggers.Length;
            // Collect all objects touched with finger colliders.
            for (int i = 0; i < n; i++)
            {
                IFingerTriggerColliderManager fingerTrigger = _fingerTriggers[i];
                if (fingerTrigger == null)
                {
                    Debug.LogError("Finger trigger is null.");
                    return;
                }
                HashSet<ITouchableForceFeedback> fingerTouchableSet;
                if (fingerTrigger.CheckTouching(out fingerTouchableSet))
                {
                    _touchableSet.UnionWithCast(fingerTouchableSet);
                }
            }
            // Collect all objects touched by picking prediction indicator.
            if (_pickingPredictionIndicatorManager != null)
            {
                HashSet<ITouchableForceFeedback> triggeringTouchable =
                    _pickingPredictionIndicatorManager.GetTriggeringObject();
                _touchableSet.UnionWithCast(triggeringTouchable);
            }
            // Update color.
            _colorModifier.UpdateTouchedObjectColor(_touchableSet);
        }
        #endregion
    }
}