/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Pickers;
using UnityEngine;

namespace Libdexmo.Unity.Snapping
{
    /// <summary>
    /// This scripts gives snapping behaviour for knob game objects. Hand models will
    /// be snapped to a fixed position but are allowed to rotate only in the plane
    /// normal to the knob's rotation axis.
    /// </summary>
    public class SnappableKnobForDexmo: SnappableCommonForDexmo
    {
        /// <summary>
        /// This function is called for every FixedUpdate cycle. It moves the snapper
        /// position towards the target position, but allows it to rotate in the plane
        /// normal to the knob's rotation axis, which is assumed to be the local "up" direction
        /// of the snapping rotation reference transform.
        /// </summary>
        /// <param name="snapper">The snapper in snapping region.</param>
        public override void OnSnappedStay(ISnapper snapper)
        {
            SnapperDexmo snapperDexmo = snapper as SnapperDexmo;
            if (snapperDexmo == null)
            {
                base.OnSnappedStay(snapper);
                return;
            }
            Transform positionReference = GetSnappingPositionReference(snapper);
            Transform rotationReference = GetSnappingRotationReference(snapper);
            if (positionReference == null || rotationReference == null)
            {
                return;
            }
            Transform palmCenterTransform = snapperDexmo.PalmCenter;
            // Find the deltaRotation in world needed to align palmCenterTransform to rotationReference
            // in only y-axis (i.e. "up" direction).
            Quaternion deltaRotation = 
                Quaternion.FromToRotation(palmCenterTransform.up, rotationReference.up);
            Quaternion palmCenterTargetRotation = deltaRotation * palmCenterTransform.rotation;
            // Now palm center's local "up" direction will coincide with rotationReference.up,
            // and can rotate around the "up" axis.

            // Move the entire hand root transform to the target position and rotation calculated from
            // the target position and rotation of the palm center. 
            Miscellaneous.MoveParentTransformGivenChildTransform(
                snapperDexmo.HandRootTransform, palmCenterTransform,
                positionReference.position, palmCenterTargetRotation,
                HandRootPositionRelativeToPalmCenter, HandRootRotationRelativeToPalmCenter);
        }
    }
}
