/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Pickers;
using Libdexmo.Unity.Triggering;
using UnityEngine;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// This script gives knob behaviour with boundary event triggers. The event is 
    /// triggered when the knob's rotation angle reaches either of angle start bound
    /// or angle end bound.
    /// </summary>
    /// <remarks>
    /// The trigger has two states, "start" and "end". When the state is "start" and
    /// knob is turned beyond the angle end bound, the state is changed to "end" and
    /// event is triggered. If the state is "end" and the knob is turned below the
    /// angle start bound, the state is changed to "start" and the event is triggered.
    /// </remarks>
    public class KnobWithBoundaryTriggerController : KnobController,
        ITwoBoundaryTrigger
    {
        /// <summary>
        /// This event is triggered when knob's rotation angle reaches one of the 
        /// bounds. When the state is "start" and knob is turned beyond the angle 
        /// end bound, the state is changed to "end" and event is triggered. If 
        /// the state is "end" and the knob is turned below the angle start bound, 
        /// the state is changed to "start" and the event is triggered. The event
        /// contains the current trigger state.
        /// </summary>
        public event EventHandler<TwoBoundaryTriggerEventArgs> TriggerStatusChanged;
        /// <summary>
        /// The trigger state can be either "start" or "end".
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
        /// Update the trigger state from the current angle of rotation.
        /// </summary>
        void FixedUpdate()
        {
            UpdateTriggerState(LastPickableAngle);
        }

        /// <summary>
        /// Trigger the TrigerStatusChanged event.
        /// </summary>
        protected virtual void OnTriggerStatusChanged()
        {
            TwoBoundaryTriggerEventArgs args = 
                new TwoBoundaryTriggerEventArgs(_curTriggerState);
            Miscellaneous.InvokeEvent(TriggerStatusChanged, this, args);
        }

        /// <summary>
        /// Update the trigger state from the current rotation angle.When the state
        /// is "start" and knob is turned beyond the angle end bound, the state is 
        /// changed to "end" and event is triggered. If the state is "end" and the 
        /// knob is turned below the angle start bound, the state is changed to 
        /// "start" and the event is triggered. The event contains the current 
        /// trigger state.
        /// </summary>
        /// <param name="currentAngle">Current knob's rotation angle.</param>
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
