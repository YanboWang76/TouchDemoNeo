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
    /// This script gives linear slider behaviour with event triggers. An event is
    /// triggered when the slider is pushed to a certain amount.
    /// </summary>
    /// <remarks>
    /// The trigger has two states, "start" and "end". When it is "start" state and
    /// the position of this object goes beyond end bound, the trigger state is 
    /// changed to "end" and the event is triggered. When the state is "end" and
    /// the position of this object goes below start bound, the state is changed
    /// to "start" and the event is triggered.
    /// </remarks>
    public class LinearSliderWithBoundaryTriggerController:
        LinearSliderController, ITwoBoundaryTrigger
    {
        /// <summary>
        /// This event is triggered when the trigger state changes. When it is 
        /// "start" state and the position of this object goes beyond end bound, 
        /// the trigger state is changed to "end" and the event is triggered.
        /// When the state is "end" and the position of this object goes below 
        /// start bound, the state is changed to "start" and the event is triggered.
        /// The event contains the current trigger state.
        /// </summary>
        public event EventHandler<TwoBoundaryTriggerEventArgs> TriggerStatusChanged; 
        /// <summary>
        /// The current trigger state. It can be either "start" or "end".
        /// </summary>
        public TwoBoundaryTriggerState TriggerState { get { return _curTriggerState; } }

        [Tooltip("The start bound of slider. When the trigger state is \"end\" and " +
                 "slider position is below this bound along the axis, the event is " +
                 "triggered.")]
        [SerializeField]
        private Transform _startBound;
        [Tooltip("The end bound of slider. When the trigger state is \"start\" and " +
                 "slider position is beyond this bound along the axis, the event is " +
                 "triggered.")]
        [SerializeField]
        private Transform _endBound;

        private float _startBoundLinearDistanceNormalized;
        private float _endBoundLinearDistanceNormalized;
        private TwoBoundaryTriggerState _curTriggerState;

        protected override void Init()
        {
            base.Init();
            Vector3 startBoundPositionInParentCoordinate =
                InverseTransformPointInParentCoordinate(_startBound.position);
            Vector3 endBoundPositionInParentCoordinate =
                InverseTransformPointInParentCoordinate(_endBound.position);
            _startBoundLinearDistanceNormalized = Vector3.Dot(
                startBoundPositionInParentCoordinate - StartPointInParentCoordinate,
                LinearMoveAxis) / LinearRange;
            _startBoundLinearDistanceNormalized = 
                Mathf.Clamp01(_startBoundLinearDistanceNormalized);
            _endBoundLinearDistanceNormalized = Vector3.Dot(
                endBoundPositionInParentCoordinate - StartPointInParentCoordinate,
                LinearMoveAxis) / LinearRange;
            _endBoundLinearDistanceNormalized = 
                Mathf.Clamp01(_endBoundLinearDistanceNormalized);
            if (_endBoundLinearDistanceNormalized < _startBoundLinearDistanceNormalized)
            {
                Debug.LogError("End bound cannot be closer to the starting point than" +
                    " the start bound.");
            }
        }

        /// <summary>
        /// Calculate the position to move based on the picker's current position
        /// during grasping. Update the trigger state if needed.
        /// </summary>
        /// <param name="picker">Picker that is grasping this object.</param>
        protected override void MoveTowardsTargetWithConstraint(IPicker picker)
        {
            Transform pickerTransform = picker.Transform;
            float currentMovement = GetPickerMovement(pickerTransform);
            transform.localPosition = StartPointInParentCoordinate +
                currentMovement * LinearRange * LinearMoveAxis;
            UpdateTriggerState(currentMovement);
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
        /// Update the trigger state. If the state is "start" and the position of
        /// slider is beyond the end bound, the state is changed to "end" and the
        /// event is triggered. If the state is "end" and the position of the
        /// slider is below the start bound, the state is changed to "start" and the
        /// event is triggered.
        /// </summary>
        /// <param name="movementNormalized">Normalized value between 0 and 1
        /// representing the position along the linear moving axis. If the value
        /// is 0, it means the point is on the start position of the line. If
        /// it is 1, it means the point is on the end position of the line.</param>
        private void UpdateTriggerState(float movementNormalized)
        {
            switch (_curTriggerState)
            {
                case TwoBoundaryTriggerState.Start:
                    if (movementNormalized >= _endBoundLinearDistanceNormalized)
                    {
                        _curTriggerState = TwoBoundaryTriggerState.End;
                        OnTriggerStatusChanged();
                    }
                    break;

                case TwoBoundaryTriggerState.End:
                    if (movementNormalized <= _startBoundLinearDistanceNormalized)
                    {
                        _curTriggerState = TwoBoundaryTriggerState.Start;
                        OnTriggerStatusChanged();
                    }
                    break;
            }
        }
    }
}