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
    /// This class is the base class for all pickable objects that will be grasped
    /// with certain physical constraints. It will also constrain the movement of
    /// the picker once it is grasped, so the picker stays with this pickable object
    /// visually.
    /// </summary>
    /// <remarks>
    /// when it is grasped, instead of completely following the motion of the picker,
    /// it partially does. Think of the lever that has its handle rotated to certain
    /// degree with constraints.
    /// </remarks>
    public abstract class PickableWithConstraint: Pickable, IPickableConstrainPicker
    {
        protected Vector3 PickerFixedDisplacementWrtConstrainPoint { get; private set; }
        protected Quaternion PickerFixedAngleDisplacementWrtConstrainPoint { get; private set; }
        protected Quaternion InitialPickerRotation { get; private set; }
        protected Transform ConstrainPositionReference { get { return _constrainPositionReference; } }
        protected Transform ConstrainRotationReference { get { return _constrainRotationReference; } }
        protected PickerDexmoConstrainPartType PickerDexmoConstrainPart
        {
            get { return _pickerDexmoConstrainPart; }
            set { _pickerDexmoConstrainPart = value; }
        }
        protected Vector3 HandRootPositionRelativeToPicker { get; private set; }
        protected Quaternion HandRootRotationRelativeToPicker { get; private set; }

        [SerializeField]
        private Transform _constrainPositionReference;
        [SerializeField]
        private Transform _constrainRotationReference;

        private PickerDexmoConstrainPartType _pickerDexmoConstrainPart;

        protected override void Init()
        {
            base.Init();
            PickerDexmoConstrainPart = PickerDexmoConstrainPartType.Wrist;
        }

        /// <summary>
        /// This function should define how pickable moves given the position of picker
        /// tracked in reality.
        /// </summary>
        /// <param name="picker">The picker that is holding this object.</param>
        protected abstract void MoveTowardsTargetWithConstraint(IPicker picker);

        /// <summary>
        /// It is called when this object is just picked up. It records some initial
        /// position and rotation offset with the picker, which will be used later.
        /// </summary>
        /// <remarks>
        /// Note that the implementation of this function is a little different from
        /// that of <see cref="Pickable"/>. Here it does not need to change the rigidbody properties
        /// to kinematic in order to follow the movement of the picker. Its movement
        /// is entirely defined in function <see cref="MoveTowardsTargetWithConstraint"/>.
        /// </remarks>
        /// <param name="picker">The picker that picks it up.</param>
        public override void OnPickedInit(IPicker picker)
        {
            Rigidbody activeRb = GetActiveRb();
            // Need to update the pickable mapping whenever a new object is picked up.
            PickableMapping.UpdatePickableMapping(activeRb, true, picker);
            PickerDexmo pickerDexmo = picker as PickerDexmo;
            Transform pickerReference = picker.Transform;
            if (pickerDexmo != null)
            {
                Transform handRootTransform = pickerDexmo.HandRootTransform;
                if (_pickerDexmoConstrainPart == PickerDexmoConstrainPartType.PalmCenter)
                {
                    pickerReference = pickerDexmo.PalmCenter;
                }
                HandRootPositionRelativeToPicker =
                    pickerReference.InverseTransformPoint(handRootTransform.position);
                HandRootRotationRelativeToPicker =
                    Quaternion.Inverse(pickerReference.rotation) * handRootTransform.rotation;
                
            }
            if (_constrainPositionReference != null)
            {
                PickerFixedDisplacementWrtConstrainPoint = pickerReference.position -
                    _constrainPositionReference.position;
            }
            if (_constrainRotationReference != null)
            {
                PickerFixedAngleDisplacementWrtConstrainPoint =
                    Quaternion.Inverse(_constrainRotationReference.rotation) *
                    pickerReference.rotation;
            }
            InitialPickerRotation = pickerReference.rotation;
            // Send the PickedInit event.
            OnPickedInitEvent(picker);
        }

        /// <summary>
        /// It is called at every FixedUpdate cycle. It moves according to the current position and
        /// rotation of the picker. Then it constrains the picker position and rotation by overwriting
        /// it, so it appears in graphics that the picker always stay on the surface of this pickable
        /// object.
        /// </summary>
        /// <param name="picker">The picker that is holding it.</param>
        public override void OnPickedUpdate(IPicker picker)
        {
            // Move itself according to the picker's current position and rotation. The exact movement
            // is implemented by the derived class.
            MoveTowardsTargetWithConstraint(picker);
            // Since the picker is tracked in reality, its position and rotation needs to be overwritten
            // to appear to stay on the surface of this pickable.
            ConstrainPicker(picker);
            // Send PickedUpdate event
            OnPickedUpdateEvent(picker);
        }

        /// <summary>
        /// It is called when this pickable object is released by the picker.
        /// </summary>
        /// <remarks>
        /// Note it is a little different from the implementation of <see cref="Pickable"/>. It does
        /// not need to change the rigidbody properties, since all the movement has been defined
        /// in <see cref="MoveTowardsTargetWithConstraint"/>.
        /// </remarks>
        /// <param name="picker">The picker that releases it.</param>
        public override void OnReleased(IPicker picker)
        {
            Rigidbody activeRb = GetActiveRb();
            // Need to update pickable mapping whenever a pickable object is released.
            PickableMapping.UpdatePickableMapping(activeRb, false, picker);
            // Send PickedRelease event
            OnReleasedEvent(picker);
        }

        /// <summary>
        /// It contrains the picker by overwriting its position and rotation. In this function
        /// it constrains picker by keeping picker a fixed postion and rotation offset with
        /// the position and the rotation of the constrain point reference transform.
        /// </summary>
        /// <remarks>
        /// The position and rotation offset between the picker and the constrain point reference
        /// is determined when this object is just picked up in <see cref="OnPickedInit"/>. 
        /// Then when this pickable moves towards the picker with certain constraint, the
        /// constraint reference will move accordingly. This function is called to bring back
        /// picker so it always have the some offset with the constrain reference.
        /// This is a virtual function and the mechanism to constrain picker may be overridden by
        /// derived classes.
        /// </remarks>
        /// <param name="picker">The picker to constrain</param>
        protected virtual void ConstrainPicker(IPicker picker)
        {
            PickerDexmo pickerDexmo = picker as PickerDexmo;
            Transform pickerReference = null;
            if (pickerDexmo == null)
            {
                pickerReference = picker.Transform;
            }
            else
            {
                switch (PickerDexmoConstrainPart)
                {
                    case PickerDexmoConstrainPartType.Wrist:
                        pickerReference = pickerDexmo.transform;
                        break;

                    case PickerDexmoConstrainPartType.PalmCenter:
                        pickerReference = pickerDexmo.PalmCenter;
                        break;
                }
            }
            if (_constrainPositionReference != null)
            {
                // Constrain picker's position to ensure it has fixed displacement
                // with constrain reference.
                pickerReference.position = _constrainPositionReference.position + 
                    PickerFixedDisplacementWrtConstrainPoint;
            }
            if (_constrainRotationReference != null)
            {
                // Constrain picker's position to ensure it has fixed relative rotation
                // with constrain reference.
                pickerReference.rotation = _constrainRotationReference.rotation *
                    PickerFixedAngleDisplacementWrtConstrainPoint;
            }
            else
            {
                //pickerReference.rotation = InitialPickerRotation;
            }

            if (pickerDexmo != null)
            {
                Vector3 pickerReferenceTargetPosition = pickerReference.position;
                Quaternion pickerReferenceTargetRotation = pickerReference.rotation;
                // In Unity, moving child transform will not change its parent, but
                // moving parent's transform will change all of its children, so the
                // constrained position and rotation of the hand root tranform (the
                // most parent transform) needs to be calculated and modified to constrain
                // the entire hand model. Target position and rotation of hand root
                // transform can be calculated from the target position and rotation of
                // its children, e.g. palm center. 
                Miscellaneous.MoveParentTransformGivenChildTransform(
                    pickerDexmo.HandRootTransform, pickerReference,
                    pickerReferenceTargetPosition, pickerReferenceTargetRotation,
                    HandRootPositionRelativeToPicker, HandRootRotationRelativeToPicker);
            }
        }
        
    }
}
