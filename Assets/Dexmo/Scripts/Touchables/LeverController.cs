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
    /// This script defines the lever behaviour. When it is grasped, it will rotate around a 
    /// rotation axis based on the current position of the picker. The centre of rotation should be
    /// the game object with this script attached and will not change in its parent's coordinate. This
    /// script will then constrain picker to make it stay with the edge of the lever by overwriting
    /// its position and rotation.
    /// </summary>
    public class LeverController : RotaryController
    {
        /// <summary>
        /// Find the picker's angle of rotation from zero.
        /// </summary>
        /// <remarks>
        /// Once the picker is grasping the pickable object, it will stay on the "surface" of the
        /// pickable by maintaining a fixed displacement with a constrain point. The constrain point
        /// is a game object that will rotate together with this one. E.g. it can be the edge of the
        /// lever. When picker is moved to new position, the position of the new constrain point can
        /// be found in turn. Picker's angle of rotation can be calculated from the angle that needs
        /// to rotate from zero deg to the direction from centre of rotation to new constrain point.
        /// </remarks>
        /// <param name="picker">The picker that is grasping the object.</param>
        /// <returns>Picker's angle of rotation from zero.</returns>
        protected override float GetPickerAngleInRotationAxis(IPicker picker)
        {
            PickerDexmo pickerDexmo = picker as PickerDexmo;
            Transform pickerReference = pickerDexmo == null ? picker.Transform : pickerDexmo.PalmCenter;

            // We want the picker transform to maintain a fixed displacement in world space with the
            // constrain point, so when picker transform is moved to a new point, the new
            // constrain point can be obtained by subtracting the fixed displacement from
            // the picker transform.
            Vector3 newConstrainPoint = pickerReference.position - PickerFixedDisplacementWrtConstrainPoint;

            // Since the object itself is the centre of rotation and will rotate during runtime,
            // all the directions here is calculated in parent transform's coordinate.

            // transform.position is the centre of rotation. Find the direction pointing from
            // centre of rotation to new constrain point position.
            Vector3 pickerRelativeDirectionInParentCoordinate =
                Miscellaneous.InverseTransformDirectionInParentCoordinate(transform,
                    newConstrainPoint - transform.position);
            // Find the angle of rotation from zero reference to the direction connecting the
            // centre of rotation to new constrain point position.
            float angle = RotationUtils.AngleInPlane(LocalRotationAxisInParentCoordinate,
                ZeroReferenceDirectionInParentCoordinate, pickerRelativeDirectionInParentCoordinate);
            return angle;
        }

        protected override void Init()
        {
            base.Init();
            PickerDexmoConstrainPart = PickerDexmoConstrainPartType.PalmCenter;
        }

        void Awake()
        {
            Init();
        }
    }
}
