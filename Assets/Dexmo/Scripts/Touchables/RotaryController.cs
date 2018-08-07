/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Pickers;
using UnityEngine;

namespace Libdexmo.Unity.Touchables.Pickables
{
    public enum PickerDexmoConstrainPartType
    {
        Wrist,
        PalmCenter
    }

    /// <summary>
    /// This abstract class is the base class of all pickable with constraint scripts
    /// that will only rotate with the picker when it is grasped, such as lever and
    /// knob.
    /// </summary>
    public abstract class RotaryController: PickableWithConstraint
    {
        protected Quaternion InitialLocalRotation { get; set; }
        protected float PickableAngleOffset { get; set; }
        protected float LastPickableAngle { get; set; }
        protected float PickerAngleOffset { get; set; }
        protected Vector3 LocalRotationAxis { get { return _localRotationAxis; } }
        protected Vector3 LocalRotationAxisInParentCoordinate
        { get { return _localRotationAxisInParentCoordinate; } }
        protected Vector3 ZeroReferenceDirection { get { return _zeroReferenceDirection; } }
        protected Vector3 ZeroReferenceDirectionInParentCoordinate
        { get { return _zeroReferenceDirectionInParentCoordinate; } }
        protected float AngleLimit { get { return _angleLimit; } }
        protected bool UnlimitedRotation { get; set; }

        // Rotation axis in local coordinate
        [SerializeField]
        private Vector3 _localRotationAxis = new Vector3(1, 0, 0);
        // The direction that marks the 0 deg of rotation
        [SerializeField]
        private Vector3 _zeroReferenceDirection = new Vector3(0, 0, -1);
        // This is the maximum angle it can rotate when grasped
        [SerializeField]
        [Range(0, 360)]
        private float _angleLimit = 180f;
        private Vector3 _localRotationAxisInParentCoordinate;
        private Vector3 _zeroReferenceDirectionInParentCoordinate;

        protected override void Init()
        {
            base.Init();
            // Find the local rotation axis in parent transform's coordinate.
            // This helps to calculate the rotation axis no matter how its parent
            // transform moves.
            _localRotationAxisInParentCoordinate =
                TransformDirectionFromLocalToParent(_localRotationAxis);
            // Find the direction in the parent transform's coordinate for the
            // same reason
            _zeroReferenceDirectionInParentCoordinate =
                TransformDirectionFromLocalToParent(_zeroReferenceDirection);
            InitialLocalRotation = transform.localRotation;
            UnlimitedRotation = false;
        }

        /// <summary>
        /// Reset the rotation of the transform. Can only be called when the pickable
        /// object is not grasped.
        /// </summary>
        public virtual void ResetRotation()
        {
            if (IsPicked)
            {
                return;
            }
            transform.localRotation = InitialLocalRotation;
            LastPickableAngle = GetCurrentAngle();
        }

        /// <summary>
        /// It is called when this object is grasped. It will record how much the picker
        /// and the object itself has rotated from the zero reference, which will be
        /// used later.
        /// </summary>
        /// <param name="picker">The picker that grasps this object.</param>
        public override void OnPickedInit(IPicker picker)
        {
            base.OnPickedInit(picker);
            // This is how much picker has rotated from the zero reference
            PickerAngleOffset = GetPickerAngleInRotationAxis(picker);
            // This is how much the pickable object itself has rotated from the zero
            // reference
            PickableAngleOffset = GetCurrentAngle();
            LastPickableAngle = PickableAngleOffset;
            if (UnlimitedRotation)
            {
                // Need to reset initial rotation variables if allowing unlimited rotation
                _localRotationAxisInParentCoordinate =
                    TransformDirectionFromLocalToParent(_localRotationAxis);
                _zeroReferenceDirectionInParentCoordinate =
                    TransformDirectionFromLocalToParent(_zeroReferenceDirection);
                InitialLocalRotation = transform.localRotation;
            }
        }

        /// <summary>
        /// Calculate the angle it needs to rotate based on the current
        /// position of the picker and rotate itself from zero reference to that
        /// angle
        /// </summary>
        /// <param name="picker">The picker that is grasping the object.</param>
        protected override void MoveTowardsTargetWithConstraint(IPicker picker)
        {
            // Calculate the angle it needs to rotate based on the position
            // of the picker
            float currentAngle = GetAdjustedCurrentAngleFromPicker(picker);
            // Restore the itself to zero rotation and rotate from there
            transform.localRotation = InitialLocalRotation;
            transform.Rotate(_localRotationAxis, currentAngle, Space.Self);
            // Save its current angle of rotation
            LastPickableAngle = currentAngle;
        }

        /// <summary>
        /// Calculate the angle it needs to rotate fromm the position of picker.
        /// </summary>
        /// <param name="picker">Picker that is grasping the object</param>
        /// <returns>Angle it needs to rotate itself</returns>
        protected float GetAdjustedCurrentAngleFromPicker(IPicker picker)
        {
            // Get picker's current angle of rotation from zero
            float pickerAngle = GetPickerAngleInRotationAxis(picker);
            // Get how much picker has rotated from the start of grasping
            float angleDiff = pickerAngle - PickerAngleOffset;
            // Decide which way to rotate by adjusting the target angle into range of
            // (reference - 180, reference + 180] such that it can be seen from the
            // adjusted angle which side of the reference the angle is. In this case,
            // target angle is PickableAngleOffset + angleDiff. The reference angle
            // is LastPickableAngle.
            float currentAngle = RotationUtils.AdjustAngleByReference(
                PickableAngleOffset + angleDiff, LastPickableAngle);
            // Clamp current angle to angle limit
            currentAngle = UnlimitedRotation 
                ? Mathf.Clamp(currentAngle, -360, 360)
                : Mathf.Clamp(currentAngle, 0, _angleLimit);
            return currentAngle;
        }

        /// <summary>
        /// Get this pickable object's current angle of rotation from zero.
        /// </summary>
        /// <returns>The current angle of rotation.</returns>
        private float GetCurrentAngle()
        {
            // _zeroReferenceDirection is in local local coordinate, so it will rotate together with
            // the pickable object, so _zeroReferenceDirection is more like the minute hand in clock
            // that makes an angle with the zero direction determined by _zeroReferenceDirectionInParentCoordinate.
            Vector3 currentRotationReferenceInParent = TransformDirectionFromLocalToParent(
                _zeroReferenceDirection);
            // Get the angle of rotation from zero in the plane normal to the rotation axis
            float angle = RotationUtils.AngleInPlane(_localRotationAxisInParentCoordinate,
                _zeroReferenceDirectionInParentCoordinate, currentRotationReferenceInParent);
            return angle;
        }

        /// <summary>
        /// Defines how picker angle is calculated. Implementation varies from different
        /// derived classes.
        /// </summary>
        /// <param name="picker">The picker that is grasping this object.</param>
        /// <returns>Picker angle of rotation.</returns>
        protected abstract float GetPickerAngleInRotationAxis(IPicker picker);

        /// <summary>
        /// Transforms the direction in local coordinate to parent transform's coordinate.
        /// </summary>
        /// <param name="localDirection">The direction vector in local coordinate.</param>
        /// <returns>The vector in parent transform's coordinate.</returns>
        protected Vector3 TransformDirectionFromLocalToParent(Vector3 localDirection)
        {
            return Miscellaneous.TransformDirectionFromLocalToParent(transform, localDirection);
        }
    }
}
