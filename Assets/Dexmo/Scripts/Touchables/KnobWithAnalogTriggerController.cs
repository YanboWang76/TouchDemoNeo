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
    /// This script gives knob behaviour with event triggered when knob is turned.
    /// The event includes a float value range between -1 and 1, indicating how much
    /// knob has been turned.
    /// </summary>
    /// <remarks>
    /// If UnlimitedRotation is false, the analog value reflects how much the knob
    /// has been turned from a zero reference, and the value is always between 0 and 1.
    /// If UnlimitedRotation is true, the analog value of the knob reflects how much
    /// knob has been turned since it was grasped. So once new grasp occurs, it sees
    /// the angle of grasping as 0 and analog value is the change from that instead of
    /// the change from a fixed zero reference.
    /// </remarks>
    public class KnobWithAnalogTriggerController: KnobController,
        IAnalogTrigger
    {
        /// <summary>
        /// This event is triggered when the angle of rotation changes. It contains
        /// the current value between -1 and 1 that indicates how much knob has been
        /// turned.
        /// </summary>
        public event EventHandler<AnalogTriggerEventArgs> TriggerStatusChanged;
        /// <summary>
        /// Value between 0 and 1 that indicates how much knob has been turned from
        /// zero reference if UnlimitedRotation is false. Value between -1 and 1
        /// indicating the change from angle of grasping if UnlimitedRotation is true.
        /// </summary>
        public float AnalogValue { get { return _analogValue; } }

        private float _analogValue = 0f;

        /// <summary>
        /// Trigger the TriggerStatusChanged event.
        /// </summary>
        protected virtual void OnTriggerStatusChanged()
        {
            AnalogTriggerEventArgs args = new AnalogTriggerEventArgs(_analogValue);
            Miscellaneous.InvokeEvent(TriggerStatusChanged, this, args);
        }

        /// <summary>
        /// Reset the rotation of knob and trigger event if needed. Only has effect when
        /// knob is not grasped.
        /// </summary>
        public override void ResetRotation()
        {
            base.ResetRotation();
            float lastAnalogValue = _analogValue;
            _analogValue = 0;
            if (Mathf.Abs(_analogValue - lastAnalogValue) > float.Epsilon)
            {
                OnTriggerStatusChanged();
            }
        }

        /// <summary>
        /// Check if the knob's rotation angle has changed. If changed, update the 
        /// trigger state.
        /// </summary>
        void FixedUpdate()
        {
            UpdateTriggerState();
        }

        /// <summary>
        /// Check the current rotation angle and update the trigger state if there is
        /// any change.
        /// </summary>
        private void UpdateTriggerState()
        {
            float lastAnalogValue = _analogValue;
            if (UnlimitedRotation)
            {
                _analogValue = LastPickableAngle > 180 
                    ? (LastPickableAngle - 360) / 180 
                    : LastPickableAngle / 180;
            }
            else
            {
                _analogValue = LastPickableAngle / AngleLimit;
            }
            if (Mathf.Abs(_analogValue - lastAnalogValue) > float.Epsilon)
            {
                OnTriggerStatusChanged();
            }
        }

        /// <summary>
        /// Called when the picker releases the knob.
        /// </summary>
        /// <param name="picker">Picker that releases the knob.</param>
        public override void OnReleased(IPicker picker)
        {
            base.OnReleased(picker);
            _analogValue = 0;
        }
    }
}
