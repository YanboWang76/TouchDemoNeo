/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core.HandController;
using Libdexmo.Unity.Core.Utility;
using UnityEngine;

namespace Libdexmo.Unity.Snapping
{
    /// <summary>
    /// This scripts provides the most common snappable behaviour for hand models. 
    /// It allows the snapper to be snapped to a fixed position and rotation.
    /// </summary>
    public class SnappableCommonForDexmo: Snappable
    {
        /// <summary>
        /// Constraint imposed on finger rotation of hand models during snapping.
        /// </summary>
        /// <remarks>
        /// This reduces degree of freedoms when hand model is interacting with
        /// other things, so it makes hand models' interaction more predictable.
        /// </remarks>
        public HandRotationNormalizedRangeWithConstraint HandConstraint
        { get { return _handConstraint; } }

        protected Vector3 HandRootPositionRelativeToPalmCenter { get; private set; }
        protected Quaternion HandRootRotationRelativeToPalmCenter { get; private set; }

        [Tooltip("Impose constraint on finger rotation of hand models during snapping.")]
        [SerializeField]
        private HandRotationNormalizedRangeWithConstraint _handConstraint;

        /// <summary>
        /// Get the snapping position reference for hand models.
        /// </summary>
        /// <param name="snapper">The snapper in snapping region.</param>
        /// <returns>Transform of snapping position reference.</returns>
        public override Transform GetSnappingPositionReference(ISnapper snapper)
        {
            SnapperDexmo snapperDexmo = snapper as SnapperDexmo;
            if (snapperDexmo == null)
            {
                return base.GetSnappingPositionReference(snapper);
            }
            return snapperDexmo.IsRight
                ? SnappingPositionReferenceList[0]
                : SnappingPositionReferenceList[1];
        }

        /// <summary>
        /// Get the snapping rotation reference for hand models.
        /// </summary>
        /// <param name="snapper">The snapper in snapping region.</param>
        /// <returns>Transform of snapping position reference..</returns>
        public override Transform GetSnappingRotationReference(ISnapper snapper)
        {
            SnapperDexmo snapperDexmo = snapper as SnapperDexmo;
            if (snapperDexmo == null)
            {
                return base.GetSnappingRotationReference(snapper);
            }
            return snapperDexmo.IsRight
                ? SnappingRotationReferenceList[0]
                : SnappingRotationReferenceList[1];
        }

        /// <summary>
        /// This function will be called when snapper just enters the snapping
        /// region. It sets some variables for later use.
        /// </summary>
        /// <param name="snapper">The snapper that just enters the snapping region.</param>
        public override void OnSnappedEnter(ISnapper snapper)
        {
            SnapperDexmo snapperDexmo = snapper as SnapperDexmo;
            if (snapperDexmo == null)
            {
                base.OnSnappedEnter(snapper);
                return;
            }
            Transform handRootTransform = snapperDexmo.HandRootTransform;
            Transform palmCenter = snapperDexmo.PalmCenter;
            if (handRootTransform != null)
            {
                HandRootPositionRelativeToPalmCenter = palmCenter.InverseTransformPoint(
                    handRootTransform.position);
                HandRootRotationRelativeToPalmCenter = Quaternion.Inverse(palmCenter.rotation) *
                    handRootTransform.rotation;
            }
        }

        /// <summary>
        /// This function will be called for every FixedUpdate cycle when snapper is
        /// in the snapping region. It finds the target snapping position and rotation
        /// for the hand model and move the hand towards it.
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
            Transform rotationReference = GetSnappingRotationReference(snapper);
            if (positionReference == null || rotationReference == null)
            {
                return;
            }
            Miscellaneous.MoveParentTransformGivenChildTransform(
                snapperDexmo.HandRootTransform, snapperDexmo.PalmCenter,
                positionReference.position, rotationReference.rotation,
                HandRootPositionRelativeToPalmCenter, HandRootRotationRelativeToPalmCenter);
        }

        /// <summary>
        /// Check if the snapper is still moving towards the target position.
        /// </summary>
        /// <remarks>
        /// The snapper is in transition when the distance between the snapper and
        /// the target snapping position is greater than _snappingTransitionDistanceMin.
        /// </remarks>
        /// <param name="snapper">The snapper in the snapping region.</param>
        /// <returns>True if snapper is still moving towards the target position.</returns>
        public override bool CheckInSnappingTransition(ISnapper snapper)
        {
            SnapperDexmo snapperDexmo = snapper as SnapperDexmo;
            if (snapperDexmo == null)
            {
                return base.CheckInSnappingTransition(snapper);
            }
            Transform positionReference = GetSnappingPositionReference(snapper);
            if (positionReference == null)
            {
                return false;
            }
            float dist = Vector3.Distance(positionReference.position,
                snapperDexmo.PalmCenter.position);
            bool inSnappingTransition = dist >= SnappingTransitionDistanceMin;
            return inSnappingTransition;
        }
    }
}
