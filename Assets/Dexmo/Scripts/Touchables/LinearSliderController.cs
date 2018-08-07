/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using Debug = UnityEngine.Debug;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// This script gives linear slider behaviour. When grasped, the picker and this
    /// object will move only in a straight line.
    /// </summary>
    public class LinearSliderController : PickableWithConstraint
    {
        protected Vector3 StartPointInParentCoordinate
        { get { return _startPointInParentCoordinate; } }
        protected Vector3 EndPointInParentCoordinate
        { get { return _endPointInParentCoordinate; } }
        protected Vector3 LinearMoveAxis { get { return _linearMoveAxis; } }
        protected float LinearRange { get { return _linearRange; } }

        [Tooltip("Start point of the line on which the picker will be constrained " +
                 "during grasping.")]
        [SerializeField]
        private Transform _startPoint;
        [Tooltip("End point of the line on which the picker will be constrained " +
                 "during grasping.")]
        [SerializeField]
        private Transform _endPoint;

        private Vector3 _pickerPositionOffset;
        private Vector3 _startPointInParentCoordinate;
        private Vector3 _endPointInParentCoordinate;
        private Vector3 _linearMoveAxis;
        private float _linearRange;

        protected override void Init()
        {
            base.Init();
            _startPointInParentCoordinate = InverseTransformPointInParentCoordinate(
                _startPoint.position);
            _endPointInParentCoordinate = InverseTransformPointInParentCoordinate(
                _endPoint.position);
            Vector3 direction = _endPointInParentCoordinate - _startPointInParentCoordinate;
            _linearMoveAxis = direction.normalized;
            _linearRange = direction.magnitude;
            if (_linearRange < 1E-6f)
            {
                Debug.LogError("Start point and end point are too close to each other.");
            }
        }

        protected virtual void Awake()
        {
            Init();
        }

        /// <summary>
        /// Called when the picker just grasps this object. Set some variables for
        /// later use.
        /// </summary>
        /// <param name="picker">Picker that grasps this object.</param>
        public override void OnPickedInit(IPicker picker)
        {
            Transform pickerTransform = picker.Transform;
            base.OnPickedInit(picker);
            _pickerPositionOffset = InverseTransformPointInParentCoordinate(
                pickerTransform.position);
        }

        /// <summary>
        /// Calculate the position to move based on the picker's current position
        /// during grasping.
        /// </summary>
        /// <param name="picker">Picker that is grasping this object.</param>
        protected override void MoveTowardsTargetWithConstraint(IPicker picker)
        {
            Transform pickerTransform = picker.Transform;
            float currentMovement = GetPickerMovement(pickerTransform);
            transform.localPosition = _startPointInParentCoordinate +
                currentMovement * _linearRange * _linearMoveAxis;
        }

        /// <summary>
        /// Calculate the normalized movement of the picker in the straight line. 
        /// </summary>
        /// <remarks>
        /// It projects the current picker's position onto the line between start
        /// point and end point and find the normalized movement ranging between
        /// 0 and 1.
        /// </remarks>
        /// <param name="pickerTransform">Picker transform.</param>
        /// <returns>Normalized position of the picker ranges between 0 and 1. If
        /// it is 0, it means the picker is at the start point or beyond. If it 
        /// is 1, it means the picker is at the end point or beyond.</returns>
        protected float GetPickerMovement(Transform pickerTransform)
        {
            Vector3 pickerPositionInParent = InverseTransformPointInParentCoordinate(
                pickerTransform.position);
            Vector3 pickerMovement = pickerPositionInParent - _pickerPositionOffset;
            float movement = Vector3.Dot(pickerMovement, _linearMoveAxis) / _linearRange;
            movement = Mathf.Clamp01(movement);
            return movement;
        }

        /// <summary>
        /// Transform point in world coordinate to point in parent transform's 
        /// coordinate.
        /// </summary>
        /// <param name="worldPoint">Point in world coordinate.</param>
        /// <returns>Point in parent transform's coordinate.</returns>
        protected Vector3 InverseTransformPointInParentCoordinate(Vector3 worldPoint)
        {
            return Miscellaneous.InverseTransformPointInParentCoordinate(
                transform, worldPoint);
        }

    }
}
