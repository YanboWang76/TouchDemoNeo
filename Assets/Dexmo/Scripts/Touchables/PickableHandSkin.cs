/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Touchables;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// This class provides functionality to let "hand skin" follow the "hand skeleton".
    /// "Hand skeleton" is tracked in reality, and "hand skin" is the graphics hand model that
    /// will move to the skeleton's position. This mechanism has not been fully tested yet.
    /// </summary>
    public sealed class PickableHandSkin : PickableForceFeedback
    {
        /// <summary>
        /// The "hand skeleton" transform that is tracked in reality. Must has at least the same
        /// finger hierarchical structure as this game object.
        /// </summary>
        public Transform HandSkeleton { get { return _handSkeleton; } }

        [SerializeField]
        private Transform _handSkeleton;

        void Awake()
        {
            Init();
            MoveToSkeletonPosition();
        }

        private void MoveToSkeletonPosition()
        {
            transform.position = HandSkeleton.position;
            transform.localRotation = HandSkeleton.localRotation;
        }

        protected override Rigidbody GetActiveRb()
        {
            return Rb;
        }

        void FixedUpdate()
        {
            UpdateRotation();
        }

        /// <summary>
        /// Update the rotation of the "hand skin" transform based on that of "hand skeleton".
        /// </summary>
        private void UpdateRotation()
        {
            UpdateRotationWorker(transform, HandSkeleton);
        }

        /// <summary>
        /// Update the rotation of transform recursively.
        /// </summary>
        /// <param name="skinTransform">The skin transform following the rotation of
        /// that of the skeleton.</param>
        /// <param name="skeletonTransform">Corresponding skeleton transform.</param>
        private void UpdateRotationWorker(Transform skinTransform,
            Transform skeletonTransform)
        {
            int childCount = skinTransform.childCount;
            if (childCount == 0)
            {
                return;
            }
            int skeletonChildCount = skeletonTransform.childCount;
            if (childCount != skeletonChildCount)
            {
                throw new ArgumentException("Skin transform and skeleton transform mismatch.");
            }
            for (int i = 0; i < childCount; i++)
            {
                Transform skinChild = skinTransform.GetChild(i);
                Transform skeletonChild = skeletonTransform.GetChild(i);
                skinChild.localRotation = skeletonChild.localRotation;
                UpdateRotationWorker(skinChild, skeletonChild);
            }
        }
    }
}
