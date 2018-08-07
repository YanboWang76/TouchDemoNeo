/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using UnityEngine;

namespace Libdexmo.Unity.Pickers
{
    /// <summary>
    /// This class is the base class of all pickers. It implements <see cref="IPicker"/> interface,
    /// which interacts with <see cref="IPickable"/> objects.
    /// </summary>
    public abstract class Picker : MonoBehaviour, IPicker
    {
        #region IPicker Implementation

        /// <summary>
        /// This event is sent when this picker is forced to release the pickable object it
        /// is holding. This can happen only if <see cref="AllowSwitchingPicker"/> is true.
        /// If that flag is true and another picker is grasping the same pickable object, it
        /// will then release the pickable and let the other picker holds the object.
        /// </summary>
        public event EventHandler SwitchPickerReleaseEvent;
        /// <summary>
        /// The Transform component associated with this picker.
        /// </summary>
        public Transform Transform { get; private set; }
        /// <summary>
        /// Whether this picker is now holding any IPickable.
        /// </summary>
        public bool IsHolding { get; protected set; }
        /// <summary>
        /// If <see cref="IsHolding"/> is true, this is the picked object. Otherwise
        /// it should not be accessed. For now, one picker can only pick up one
        /// IPickable object.
        /// </summary>
        public IPickable PickedObj { get; private set; }
        /// <summary>
        /// The current velocity of the picker.
        /// </summary>
        public virtual Vector3 Velocity { get { return _dynamicsManager.Velocity; } }
        /// <summary>
        /// The current angular velocity of this picker.
        /// </summary>
        public virtual Vector3 AngularVelocity { get { return _dynamicsManager.AngularVelocity; } }
        /// <summary>
        /// Whether this picker is allowed to let go of the pickable object held when
        /// the other picker grab the pickable from it. If this is true, when this picker
        /// is holding the pickable object, the other picker can force this picker to release
        /// it by picking up the same pickable object. In this case, the picker associated with
        /// the pickable will change and <see cref="SwitchPickerRelease"/> will be called.
        /// If this is false, switching between picker will not occur.
        /// </summary>
        public bool AllowSwitchingPicker { get; protected set; }

        #endregion

        /// <summary>
        /// Whether to update velocity and angular velocity from DynamicsManager.
        /// </summary>
        protected virtual bool UsingDynamicsManager { get { return true; } }

        /// <summary>
        /// A utility class to maintain velocity and angular velocity from position and rotation.
        /// </summary>
        private DynamicsManager _dynamicsManager;

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Start()
        {
            InitDynamicsManager();
        }

        protected virtual void Init()
        {
            IsHolding = false;
            Transform = transform;
            AllowSwitchingPicker = true;
        }

        private void InitDynamicsManager()
        {
            if (UsingDynamicsManager)
            {
                _dynamicsManager = new DynamicsManager(transform.position, transform.rotation);
            }
        }

        /// <summary>
        /// Release the pickable object held if any.
        /// </summary>
        protected virtual void StopPicking()
        {
            if (IsHolding)
            {
                if (!PickedObj.Equals(null) && PickedObj.Picker == this)
                {
                    PickedObj.OnReleased(this);
                }
                IsHolding = false;
                PickedObj = null;
                Debug.Log("Stop picking.");
            }
        }

        /// <summary>
        /// This function will be called in FixedUpdate cycle. It calls the pickable object's
        /// OnPickedUpdate function.
        /// </summary>
        protected virtual void PickingFixedUpdate()
        {
            // Check if PickedObj has been destroyed
            if (PickedObj == null || PickedObj.Equals(null))
            {
                PickedObj = null;
                IsHolding = false;
                return;
            }
            if (IsHolding)
            {
                PickedObj.OnPickedUpdate(this);
            }
        }

        /// <summary>
        /// Try to pick up an object. Return true if successful. It may fail because the
        /// object does not have pickable script attached or the object has been picked up
        /// by other picker that whose <see cref="AllowSwitchingPicker"/> is false.
        /// </summary>
        /// <param name="pickedObjectTransform">The transform of the object that the
        /// picker tries to pick up.</param>
        /// <returns>True if picker successfully picks up.</returns>
        protected bool AttachPickedObj(Transform pickedObjectTransform)
        {
            IPickable pickable = pickedObjectTransform.GetComponentInParent<IPickable>();
            if (pickable == null)
            {
                return false;
            }
            return AttachPickable(pickable);
        }

        /// <summary>
        /// Try to pick up a pickable object. Return true if successful. It may fail because
        /// the pickable object is being held by a picker whose <see cref="AllowSwitchingPicker"/>
        /// is false.
        /// </summary>
        /// <param name="pickable">The pickable object.</param>
        /// <returns>True if successful.</returns>
        protected bool AttachPickable(IPickable pickable)
        {
            if (pickable.IsPicked)
            {
                IPicker otherPicker = pickable.Picker;
                if (otherPicker == this)
                {
                    Debug.LogError("Picking the same object.");
                    return false;
                }
                if (!otherPicker.AllowSwitchingPicker)
                {
                    return false;
                }
                else
                {
                    // Switch picker
                    otherPicker.SwitchPickerRelease();
                }
            }
            PickedObj = pickable;
            IsHolding = true;
            pickable.OnPickedInit(this);
            return true;
        }

        /// <summary>
        /// Invoke event when the this picker is forced to release the held object
        /// because other picker is grabbing the pickable object from this picker.
        /// </summary>
        protected void OnSwitchPickerRelease()
        {
            Miscellaneous.InvokeEvent(SwitchPickerReleaseEvent, this);
        }

        /// <summary>
        /// Public callback to release the current pickable object because some other
        /// picker is grabbing the pickable object from me.
        /// </summary>
        public virtual void SwitchPickerRelease()
        {
            OnSwitchPickerRelease();
            StopPicking();
        }

        /// <summary>
        /// Update the dynamics manager to get velocity and angular velocity if dynamics
        /// manager is used.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            if (_dynamicsManager != null)
            {
                _dynamicsManager.Update(transform.position, transform.rotation);
            }
        }
    }
}