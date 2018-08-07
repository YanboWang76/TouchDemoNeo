/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;
using UnityEngine;

namespace Libdexmo.Unity.Snapping
{
    /// <summary>
    /// This scripts gives snappable behaviour with constraint to move only in a
    /// line segment. When hand model is snapped to it, hand's rotation will be
    /// fixed, but hand can move in a line segment defined by the two end points
    /// of position constraint transforms.
    /// </summary>
    public class SnappableWithLinearConstraintForDexmo: SnappableCommonForDexmo
    {
        [Tooltip("One end point of the line segment on which snapped hand is allowed" +
                 " to move.")]
        [SerializeField]
        private Transform _startPositionConstraintTransform;
        [Tooltip("The other end point of the line segment on which snapped hand is" +
                 " allowed to move.")]
        [SerializeField]
        private Transform _endPositionConstraintTransform;

        /// <summary>
        /// This function will be called for every FixedUpdate cycle when snapper
        /// is in snapping region. It fixes the snapper's rotation to that of
        /// snapping rotation reference and allows the snapper to move between
        /// start position constraint and end position constraint.
        /// </summary>
        /// <param name="snapper">The snapper in the snapping region.</param>
        public override void OnSnappedStay(ISnapper snapper)
        {
            SnapperDexmo snapperDexmo = snapper as SnapperDexmo;
            if (snapperDexmo == null)
            {
                base.OnSnappedStay(snapper);
                return;
            }
            Transform positionReference = GetSnappingPositionReference(snapper);

            // Adjust snapping position according to the current position of the snapper.
            // Snapping position will only move in a linear section between start point and
            // end point.
            if (positionReference == null)
            {
                return;
            }
            float vectorProjectionValueNormalized = 
                Miscellaneous.GetVectorProjectionValueNormalized(
                    _startPositionConstraintTransform.position,
                    _endPositionConstraintTransform.position,
                    snapperDexmo.PalmCenter.position);
            positionReference.position = Vector3.Lerp(
                _startPositionConstraintTransform.position,
                _endPositionConstraintTransform.position,
                vectorProjectionValueNormalized);
            Transform rotationReference = GetSnappingRotationReference(snapper);
            if (rotationReference == null)
            {
                return;
            }
            
            // Move the entire hand root transform to the target position and rotation calculated from
            // the target position and rotation of the palm center. 
            Miscellaneous.MoveParentTransformGivenChildTransform(
                snapperDexmo.HandRootTransform, snapperDexmo.PalmCenter,
                positionReference.position, rotationReference.rotation,
                HandRootPositionRelativeToPalmCenter, HandRootRotationRelativeToPalmCenter);
        }
    }
}
