/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Triggering;
using UnityEngine;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// This script gives lever behaviour with event when lever is pulled down or
    /// pushed up.
    /// </summary>
    /// <remarks>
    /// When lever is grasped, it can rotate itself through certain range. This script
    /// allows two bounds. Initially, the rotation angle is 0 deg. When the angle exceeds
    /// the upper bound, or angle end bound, it will fire the event 
    /// <see cref="TriggerStatusChanged"/>. If the lever is then pushed back and angle of
    /// rotation drops below the lower bound, or angle start bound, TriggerStatusChanged
    /// event will be sent again. The event includes the current state of the lever.
    /// </remarks>
    public class LeverWithBoundaryTriggerController : LeverController,
        ITwoBoundaryTrigger
    {
        /// <summary>
        /// Triggered if the state of the lever is changed. The state changes when either
        /// the current state is "start" and angle of rotaion exceeds the angle end 
        /// bound or the current state is "end" and angle of rotation drops below
        /// angle start bound. The event contains the current trigger state.
        /// </summary>
        public event EventHandler<TwoBoundaryTriggerEventArgs> TriggerStatusChanged;
        /// <summary>
        /// The current state of the lever. It can be either "start" or "end".
        /// </summary>
        public TwoBoundaryTriggerState TriggerState { get { return _curTriggerState; } }

        [Tooltip("Lower bound. When the curernt state is \"end\" and rotation angle " +
                 "drops below this value, the TriggerStatusChanged event is triggered.")]
        [SerializeField]
        [Range(0, 360)]
        private float _angleStartBound;
        [Tooltip("Upper bound. When the current state is \"start\" and rotation angle " +
                 "exceeds this value, the TriggerStatusChanged event is triggered.")]
        [SerializeField]
        [Range(0, 360)]
        private float _angleEndBound;
        private TwoBoundaryTriggerState _curTriggerState;

        protected override void Init()
        {
            base.Init();
            if (_angleEndBound < _angleStartBound)
            {
                Debug.LogError("End bound cannot be closer to starting point than the start bound.");
            }
            _angleEndBound = Mathf.Clamp(_angleEndBound, 0, AngleLimit);
            _curTriggerState = TwoBoundaryTriggerState.Start;
        }

        /// <summary>
        /// Calculate the angle it needs to rotate based on the current
        /// position of the picker and rotate itself from zero reference to that
        /// angle. Update the trigger state from the current angle.
        /// </summary>
        /// <param name="picker">The picker that is grasping the object.</param>
        protected override void MoveTowardsTargetWithConstraint(IPicker picker)
        {
            float currentAngle = GetAdjustedCurrentAngleFromPicker(picker);
            transform.localRotation = InitialLocalRotation;
            transform.Rotate(LocalRotationAxis, currentAngle, Space.Self);
            LastPickableAngle = currentAngle;
            UpdateTriggerState(currentAngle);
        }

        /// <summary>
        /// Reset the rotation of lever and trigger event if needed. Only has effect
        /// when lever is not grasped.
        /// </summary>
        public override void ResetRotation()
        {
            base.ResetRotation();
            // LastPickableAngle should have been reset in base.ResetRotation().
            UpdateTriggerState(LastPickableAngle);
        }

        /// <summary>
        /// Trigger the TriggerStatusChanged event.
        /// </summary>
        protected virtual void OnTriggerStatusChanged()
        {
            TwoBoundaryTriggerEventArgs args =
                new TwoBoundaryTriggerEventArgs(_curTriggerState);
            Miscellaneous.InvokeEvent(TriggerStatusChanged, this, args);
        }

        /// <summary>
        /// Update the trigger state. If the current trigger state is "start" and
        /// rotation angle exceeds the angle end bound, the state is changed to
        /// "end" and event is triggered. If the current state is "end" and the 
        /// rotation angle drops below the angle sstart bound, the state is changeed
        /// to "start" and event is triggered.
        /// </summary>
        /// <param name="currentAngle">Current angle of rotation.</param>
        private void UpdateTriggerState(float currentAngle)
        {
            switch (_curTriggerState)
            {
                case TwoBoundaryTriggerState.Start:
                    if (currentAngle >= _angleEndBound)
                    {
                        _curTriggerState = TwoBoundaryTriggerState.End;
                        OnTriggerStatusChanged();
                    }
                    break;

                case TwoBoundaryTriggerState.End:
                    if (currentAngle <= _angleStartBound)
                    {
                        _curTriggerState = TwoBoundaryTriggerState.Start;
                        OnTriggerStatusChanged();
                    }
                    break;
            }
        }
    }
}
