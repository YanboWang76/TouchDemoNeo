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
    /// This script helps snapper to collect all the trigger colliders in contact.
    /// All snapper colliders must be either sphere or box colliders.
    /// </summary>
    /// <remarks>
    /// Snapping begins when trigger collider of snapper collides with the those
    /// of snappable objects and this scripts manages the collision of snapper
    /// with other snappable colliders. It checks overlap of colliders out
    /// of physics cycle by using Unity API Physics.OverlapBox and 
    /// Physics.OverlapSphere, so the colliders of snappers must be either
    /// of these two types.
    /// </remarks>
    public class SnapperTriggerColliderManager : MonoBehaviour
    {
        private List<BoxCollider> _selfBoxColliders;
        private List<SphereCollider> _selfSphereColliders;
        private HashSet<Collider> _overlappedColliderSet;

        /// <summary>
        /// This function is called when the script is first attached or
        /// when "Reset" of script is clicked. It finds the "Snapper" layer
        /// and assigns it to this game object.
        /// </summary>
        void Reset()
        {
            int snapperLayer = LayerMask.NameToLayer("Snapper");
            if (snapperLayer == -1)
            {
                Debug.LogError("Must define \"Snapper\" layer.");
            }
            else
            {
                gameObject.layer = snapperLayer;
            }
        }

        void Awake()
        {
            _selfBoxColliders = new List<BoxCollider>();
            _selfSphereColliders = new List<SphereCollider>();
            _overlappedColliderSet = new HashSet<Collider>();
            FindSelfColliders();
        }

        /// <summary>
        /// Find all box or sphere colliders attached to this game object.
        /// </summary>
        private void FindSelfColliders()
        {
            BoxCollider[] boxColliders = GetComponents<BoxCollider>();
            foreach (BoxCollider boxCollider in boxColliders)
            {
                _selfBoxColliders.Add(boxCollider);
            }
            SphereCollider[] sphereColliders = GetComponents<SphereCollider>();
            foreach (SphereCollider sphereCollider in sphereColliders)
            {
                _selfSphereColliders.Add(sphereCollider);
            }
        }

        /// <summary>
        /// Return a set of overlapping colliders with colliders attached to this
        /// game object.
        /// </summary>
        /// <returns>A set of overlapping colliders.</returns>
        public HashSet<Collider> CheckOverlapColliders()
        {
            _overlappedColliderSet.Clear();
            // Check OverlapBox for all box colliders
            int n = _selfBoxColliders.Count;
            for (int i = 0; i < n; i++)
            {
                BoxCollider boxCollider = _selfBoxColliders[i];
                Collider[] overlappedColliders = ColliderUtility.CheckOverlapBox(
                    boxCollider);
                int overlappedColliderLength = overlappedColliders.Length;
                for (int j = 0; j < overlappedColliderLength; j++)
                {
                    Collider overlappedCollider = overlappedColliders[j];
                    _overlappedColliderSet.Add(overlappedCollider);
                }
            }
            // Check OverlapSphere for all sphere colliders
            n = _selfSphereColliders.Count;
            for (int i = 0; i < n; i++)
            {
                SphereCollider sphereCollider = _selfSphereColliders[i];
                Collider[] overlappedColliders = 
                    ColliderUtility.CheckOverlapSphere(sphereCollider);
                int overlappedColliderLength = overlappedColliders.Length;
                for (int j = 0; j < overlappedColliderLength; j++)
                {
                    Collider overlappedCollider = overlappedColliders[j];
                    _overlappedColliderSet.Add(overlappedCollider);
                }
            }
            return _overlappedColliderSet;
        }
    }
}
