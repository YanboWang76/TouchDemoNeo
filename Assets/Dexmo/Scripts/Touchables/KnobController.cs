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
    /// <summary>
    /// This script defines knob behaviour. When it is grasped, it will rotate itself
    /// around an axis, based on the current position of the picker. The centre of rotation
    /// should be this game object and it will change in parent's coordinate. This script will
    /// then constrain picker such that the picker's centre will be fixed to the constrain
    /// position reference and the picker's rotation will change with this transform. The
    /// constrain position reference should be along the rotation axis.
    /// </summary>
    public class KnobController : RotaryController
    {
        // If this is true, knob can be rotated throughout 360 deg. Otherwise, its rotation
        // will be limited to AngleLimit.
        [SerializeField]
        private bool _unlimitedRotation = false;

        private Quaternion _pickerRelativeRotationWrtKnob;

        void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();
            PickerDexmoConstrainPart = PickerDexmoConstrainPartType.PalmCenter;
            UnlimitedRotation = _unlimitedRotation;
        }

        /// <summary>
        /// It will be called when this object is just picked up. It will save some relative
        /// rotation variables to be used later.
        /// </summary>
        /// <param name="picker">The picker that picks it up.</param>
        public override void OnPickedInit(IPicker picker)
        {
            PickerDexmo pickerDexmo = picker as PickerDexmo;
            Transform pickerReference =
                pickerDexmo == null ? picker.Transform : pickerDexmo.PalmCenter;
            _pickerRelativeRotationWrtKnob = 
                Quaternion.Inverse(transform.rotation) * pickerReference.rotation;
            base.OnPickedInit(picker);
        }

        /// <summary>
        /// Find the target angle of rotation from zero based on current picker's rotation. 
        /// </summary>
        /// <remarks>
        /// The picker will maintain a fixed relative rotation with knob since the start of
        /// grasping, so given the current rotation of picker, we can compute the new target
        /// rotation of knob.
        /// </remarks>
        /// <param name="picker">The picker that is grasping it.</param>
        /// <returns>Target angle of rotation of knob based on the picker's rotation.</returns>
        protected override float GetPickerAngleInRotationAxis(IPicker picker)
        {
            PickerDexmo pickerDexmo = picker as PickerDexmo;
            Transform pickerReference = pickerDexmo == null ? picker.Transform : pickerDexmo.PalmCenter;
            // Calculate the target rotation of knob based on current rotation of the picker
            Quaternion newKnobTargetRotation = pickerReference.rotation *
                Quaternion.Inverse(_pickerRelativeRotationWrtKnob);
            // Convert the current angle reference from world coordinate to parent transform's
            // coordinate. Angle reference is the direction that indicates the current angle
            // of rotation from zero.
            Vector3 newAngleReferenceDirectionInWorld = newKnobTargetRotation * ZeroReferenceDirection;
            Vector3 pickerRelativeDirectionInParentCoordinate =
                Miscellaneous.InverseTransformDirectionInParentCoordinate(transform,
                    newAngleReferenceDirectionInWorld);
            // Find the target angle of rotation from zero in the plane normal to rotation axis
            float angle = RotationUtils.AngleInPlane(LocalRotationAxisInParentCoordinate,
                ZeroReferenceDirectionInParentCoordinate, pickerRelativeDirectionInParentCoordinate);
            return angle;
        }

        /// <summary>
        /// Constrain picker such that its centre position stays at the centre of rotation and
        /// its rotation changes with the knob.
        /// </summary>
        /// <param name="picker">The picker that is grasping it.</param>
        protected override void ConstrainPicker(IPicker picker)
        {
            PickerDexmo pickerDexmo = picker as PickerDexmo;
            if (pickerDexmo == null)
            {
                //Transform pickerReference = picker.Transform;
                //if (ConstrainPositionReference != null)
                //{
                //    pickerReference.position = ConstrainPositionReference.position +
                //        PickerFixedDisplacementWrtConstrainPoint;
                //}
                //if (ConstrainRotationReference != null)
                //{
                //    pickerReference.rotation = ConstrainRotationReference.rotation *
                //        PickerFixedAngleDisplacementWrtConstrainPoint;
                //}

                // Don't constrain picker if it is not dexmo.
            }
            else
            {
                // Only constrain picker if it is dexmo
                Transform pickerReference = pickerDexmo.PalmCenter;
                Vector3 pickerReferenceTargetPostion = ConstrainPositionReference.position;
                Quaternion pickerReferenceTargetRotation = 
                    transform.rotation *
                    _pickerRelativeRotationWrtKnob;
                // In Unity, moving child transform will not change its parent, but
                // moving parent's transform will change all of its children, so the
                // constrained position and rotation of the hand root tranform (the
                // most parent transform) needs to be calculated and modified to constrain
                // the entire hand model. Target position and rotation of hand root
                // transform can be calculated from the target position and rotation of
                // its children, e.g. palm center. 
                Miscellaneous.MoveParentTransformGivenChildTransform(
                    pickerDexmo.HandRootTransform, pickerReference, pickerReferenceTargetPostion,
                    pickerReferenceTargetRotation, HandRootPositionRelativeToPicker,
                    HandRootRotationRelativeToPicker);
            }
        }
    }
}
