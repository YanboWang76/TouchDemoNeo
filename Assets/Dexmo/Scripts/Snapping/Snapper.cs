/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;

namespace Libdexmo.Unity.Snapping
{
    /// <summary>
    /// This is the base class for all snappers. Snappers are objects that can
    /// snap itself to other objects. 
    /// </summary>
    /// <remarks>
    /// For example, all hand models are attached with snapper scripts, so they
    /// can be snapped to other snappable objects. 
    /// </remarks>
    public abstract class Snapper : MonoBehaviour, ISnapper
    {
        #region ISnapper Properties
        public bool IsSnapped { get; protected set; }
        public ISnappable SnappedObject { get; private set; }
        public Transform Transform { get { return transform; } }
        #endregion

        protected List<SnapperTriggerColliderManager> TriggerColliderManagers
        { get { return _triggerColliderManagers;} }
        protected HashSet<Collider> OtherColliderSetFromTriggerColliderManagers
        { get; private set; }
        protected bool LastSnappingStatus { get; set; }

        [Tooltip("Game object with SnapperTriggerColliderManager attached to manage" +
                 " the collision between trigger colliders. When snapper's trigger" +
                 " collider collides with the collider of this game object, snapping" +
                 " occurs.")]
        [SerializeField]
        private List<SnapperTriggerColliderManager> _triggerColliderManagers;

        /// <summary>
        /// This is called when the script is first attached and when "reset" of the
        /// script is clicked.
        /// </summary>
        protected virtual void Reset()
        {
            _triggerColliderManagers = new List<SnapperTriggerColliderManager>();
            FindTriggerColliderManagers();
        }

        protected virtual void Init()
        {
            LastSnappingStatus = false;
            OtherColliderSetFromTriggerColliderManagers = new HashSet<Collider>();
        }

        /// <summary>
        /// Try to find trigger collider manager from all children transforms by.
        /// </summary>
        private void FindTriggerColliderManagers()
        {
            _triggerColliderManagers = new List<SnapperTriggerColliderManager>(
                GetComponentsInChildren<SnapperTriggerColliderManager>());
        }

        /// <summary>
        /// This function is called for every FixedUpdate cycle. It checks if
        /// snapper is now in any snapping region and if so, snaps itself to
        /// any snappable object.
        /// </summary>
        public virtual void SnapUpdate()
        {
            // Test if the snapped object is still there (i.e. not destroyed)
            if (SnappedObject != null && SnappedObject.Equals(null))
            {
                IsSnapped = false;
                SnappedObject = null;
                return;
            }
            bool isSnappedToTheSameObject;
            ISnappable firstSnappedObject;
            IsSnapped = GetSnappingStatus(out isSnappedToTheSameObject,
                out firstSnappedObject);
            if (LastSnappingStatus)
            {
                if (IsSnapped)
                {
                    if (isSnappedToTheSameObject)
                    {
                        // All is good. Don't need to do anything.
                    }
                    else
                    {
                        // Snapped to the different object. Do some finish up.
                        // OnSnappedExit for the current snapped object.
                        OnSnappedExit();
                        SnappedObject = firstSnappedObject;
                        // OnSnappedEnter for the new snapped object.
                        OnSnappedEnter();
                    }
                }
                else
                {
                    // Not snapped to the object any more.
                    OnSnappedExit();
                    SnappedObject = null;
                }
            }
            else
            {
                if (IsSnapped)
                {
                    // Start to snap to the object.
                    SnappedObject = firstSnappedObject;
                    OnSnappedEnter();
                }
                else
                {
                    // Still not snap to anything. Do nothing here.
                }
            }

            if (IsSnapped)
            {
                OnSnappedStay();
            }
            LastSnappingStatus = IsSnapped;
        }

        /// <summary>
        /// This function is called when snapper just enters the snapping
        /// region. It calls snappable object's OnSnappedEnter if any.
        /// </summary>
        protected virtual void OnSnappedEnter()
        {
            if (SnappedObject != null && !SnappedObject.Equals(null))
            {
                SnappedObject.OnSnappedEnter(this);
            }
        }

        /// <summary>
        /// This function is called for every FixedUpdate cycle when snapper
        /// stays in the snapping region. It calls snappable object's
        /// OnSnappedStay if any.
        /// </summary>
        protected virtual void OnSnappedStay()
        {
            if (SnappedObject != null && !SnappedObject.Equals(null))
            {
                SnappedObject.OnSnappedStay(this);
            }
        }

        /// <summary>
        /// This funcion is called when snapper exits the snapping region.
        /// It calls snappable object's OnSnappedExit if any.
        /// </summary>
        protected virtual void OnSnappedExit()
        {
            if (SnappedObject != null && !SnappedObject.Equals(null))
            {
                SnappedObject.OnSnappedExit(this);
            }
        }

        /// <summary>
        /// Update the set of colliders by collecting all the colliders overlapped with
        /// trigger collider managers. 
        /// </summary>
        private void UpdateOtherColliderSetFromTriggerColliderManagers()
        {
            OtherColliderSetFromTriggerColliderManagers.Clear();
            int n = _triggerColliderManagers.Count;
            for (int i = 0; i < n; i++)
            {
                SnapperTriggerColliderManager manager = _triggerColliderManagers[i];
                HashSet<Collider> overlappedColliders = manager.CheckOverlapColliders();
                foreach (Collider c in overlappedColliders)
                {
                    OtherColliderSetFromTriggerColliderManagers.Add(c);
                }
            }
        }

        /// <summary>
        /// Check if snapper should now snap to any snappable objects.
        /// </summary>
        /// <param name="isSnappedToTheSameObject"></param>
        /// <param name="firstSnappedObject"></param>
        /// <returns></returns>
        protected virtual bool GetSnappingStatus(out bool isSnappedToTheSameObject, 
            out ISnappable firstSnappedObject)
        {
            bool isSnappedThisTime = false;
            bool isSnappedLastTime = IsSnapped;
            firstSnappedObject = null;
            isSnappedToTheSameObject = false;
            UpdateOtherColliderSetFromTriggerColliderManagers();
            foreach (Collider c in OtherColliderSetFromTriggerColliderManagers)
            {
                ISnappable snappable = c.GetComponentInParent<ISnappable>();
                if (snappable == null)
                {
                    continue;
                }
                // Found a snappable object
                if (!isSnappedThisTime)
                {
                    isSnappedThisTime = true;
                    firstSnappedObject = snappable;
                }
                if (isSnappedLastTime)
                {
                    if (snappable == SnappedObject)
                    {
                        // Still snapped to the same object
                        isSnappedToTheSameObject = true;
                        break;
                    }
                    else
                    {
                        // Different from the object snapped last time. Continue the
                        // search.
                    }
                }
                else
                {
                    // Is not snapped last time, so use the first snappable as the
                    // snapped object this time.
                    break;
                }
            }

            return isSnappedThisTime;
        }
    }
}