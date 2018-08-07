/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// This abstract class is the base class of all pickable objects. It
    /// provides the common implementation IPickable interface, which is
    /// used to interact with IPicker objects. 
    /// </summary>
    public abstract class Pickable : MonoBehaviour, IPickable
    {
        #region IPickable Implementation
        public event EventHandler<PickableEventArgs> PickedInit;
        public event EventHandler<PickableEventArgs> PickedUpdate;
        public event EventHandler<PickableEventArgs> PickedReleased;

        public GameObject BindedGameObject
        {
            get { return gameObject; }
        }
        public bool IndicateColorOnTouching
        {
            get { return _indicateColorOnTouching; }
            set { _indicateColorOnTouching = value; }
        }
        public Color IndicatedColorOnTouching
        {
            get { return _indicatedColorOnTouching; }
            set { _indicatedColorOnTouching = value; }
        }
        #endregion

        [SerializeField]
        private bool _indicateColorOnTouching = false;
        [SerializeField]
        private Color _indicatedColorOnTouching = Color.white;

        /// <summary>
        /// Transform associated with this game object.
        /// </summary>
        public Transform Transform { get; private set; }
        /// <summary>
        /// Deprecated. Will be removed in the future version.
        /// </summary>
        public float SelfMass = 1f;
        /// <summary>
        /// Deprecated. Will be removed in the future version.
        /// </summary>
        public float TotalMass;

        /// <summary>
        /// Whether this object is now picked any picker.
        /// </summary>
        public virtual bool IsPicked
        {
            get
            {
                Rigidbody rb = GetActiveRb();
                return PickableMapping.CheckIsPicked(rb);
            }
        }

        /// <summary>
        /// If <see cref="IsPicked"/> is true, this is the picker that is holding
        /// this object.
        /// </summary>
        public virtual IPicker Picker
        {
            get
            {
                Rigidbody rb = GetActiveRb();
                return PickableMapping.GetAssociatedPicker(rb);
            }
        }
        /// <summary>
        /// The rigidbody that this script is associated with.
        /// </summary>
        protected Rigidbody Rb { get; set; }
        /// <summary>
        /// The relative rotation with respect to the picker transform when it is
        /// just picked up by the picker.
        /// </summary>
        /// <remarks>
        /// It is mainly used to calculate how pickable should follow the picker
        /// when it is grasped by the picker.
        /// </remarks>
        protected Quaternion RotationOffsetWithPicker { get; set; }
        /// <summary>
        /// The relative position with respect to the picker transform when it is
        /// just picked up by the picker.
        /// </summary>
        /// <remarks>
        /// It is mainly used to calculate how pickable should follow the picker
        /// when it is grasped by the picker.
        /// </remarks>
        protected Vector3 PositionOffsetWithPicker { get; set; }

        private List<Collider> _childColliderList;
        private float _velocityMax;
        private float _distMaxForFollowingAngVel;
        private RigidbodyConstraints _rbOriginalConstraints;

        protected virtual void Init()
        {
            Rb = gameObject.GetComponent<Rigidbody>();
            _childColliderList = PhysicsUtils.GetAllChildColliders(transform);
            Transform = transform;
            _velocityMax = 10;
            _distMaxForFollowingAngVel = 0.5f;
        }

        /// <summary>
        /// Make all child collider trigger colliders.
        /// </summary>
        protected void EnableTriggerAll()
        {
            foreach (Collider c in _childColliderList)
            {
                c.isTrigger = true;
            }
        }

        /// <summary>
        /// Make all child colliders collision colliders (non-trigger colliders).
        /// </summary>
        protected void DisableTriggerAll()
        {
            foreach (Collider collider in _childColliderList)
            {
                collider.isTrigger = false;
            }
        }

        /// <summary>
        /// Remove all child colliders in a particular layer.
        /// </summary>
        /// <param name="layer">Layer number</param>
        protected void RemoveChildColliderWithLayer(int layer)
        {
            List<Collider> childCollidersToBeRemoved = new List<Collider>();
            foreach (Collider child in _childColliderList)
            {
                if (child.gameObject.layer == layer)
                {
                    childCollidersToBeRemoved.Add(child);
                }
            }
            foreach (Collider child in childCollidersToBeRemoved)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Returns the associated rigidbody of this pickable object.
        /// </summary>
        /// <returns>The associated rigidbody</returns>
        protected virtual Rigidbody GetActiveRb()
        {
            return Rb;
        }

        protected virtual void OnPickedInitEvent(IPicker picker)
        {
            Miscellaneous.InvokeEvent(PickedInit, this, new PickableEventArgs(picker));
        }

        protected virtual void OnPickedUpdateEvent(IPicker picker)
        {
            Miscellaneous.InvokeEvent(PickedUpdate, this, new PickableEventArgs(picker));
        }

        protected virtual void OnReleasedEvent(IPicker picker)
        {
            Miscellaneous.InvokeEvent(PickedReleased, this, new PickableEventArgs(picker));
        }

        /// <summary>
        /// It will be called when this object is just picked up. It initializes some variables
        /// that will be used during <see cref="OnPickedUpdate"/>.
        /// </summary>
        /// <param name="picker">The picker that picks this object up.</param>
        public virtual void OnPickedInit(IPicker picker)
        {
            // Default method for pickable target
            Transform pickerTransform = picker.Transform;
            Rigidbody rb = GetActiveRb();
            // Modify the associated rigidbody, to make it kinematic rigidbody, so it will
            // follows the movement of the picker.
            PutRbUnderControlState(rb, picker);
            // Record the initial relative rotation and position with respect to the picker,
            // which will be used in OnPickedUpdate to follow the movement of the picker.
            InitPickedRb(rb, pickerTransform);
            // Send PickedInit event.
            OnPickedInitEvent(picker);
        }

        /// <summary>
        /// It will be called for every FixedUpdate cycle when this object is being held by
        /// the picker. It calculates its rigidbody's current position and rotation based on
        /// the fixed relative position and rotation recorded in <see cref="OnPickedInit"/>
        /// with respect to the picker.
        /// </summary>
        /// <param name="picker">The picker currently holding this pickable object.</param>
        public virtual void OnPickedUpdate(IPicker picker)
        {
            if (!IsPicked)
            {
                return;
            }
            Transform pickerTransform = picker.Transform;
            Rigidbody rb = GetActiveRb();
            // The position following the movement of the picker
            Vector3 calPosition = pickerTransform.position + pickerTransform.rotation * PositionOffsetWithPicker;
            // The rotation following the movement of the picker
            Quaternion calRotation = pickerTransform.rotation * RotationOffsetWithPicker;
            rb.MovePosition(calPosition);
            rb.MoveRotation(calRotation);
            //if (GameController.Instance.ForceReleaseWhenAway)
            //{
            //    if (Vector3.Distance(calPosition, rb.position) > GameController.Instance.ForceReleaseDistance)
            //    {
            //        OnReleased(picker);
            //        return;
            //    }
            //}
            //if (GameController.Instance.AllowPassThrough)
            //{
            //    rb.MovePosition(calPosition);
            //    rb.MoveRotation(calRotation);
            //}
            //else
            //{
            //    float angle;
            //    Vector3 axis;
            //    (calRotation * Quaternion.Inverse(rb.rotation)).ToAngleAxis(out angle, out axis);
            //    if (angle > 180)
            //    {
            //        angle -= 360;
            //    }
            //    // Angular velocity of the rigidbody at rb.position
            //    Vector3 angularVelocity = angle * axis / Time.fixedDeltaTime * 0.02f;
            //    // Linear velocity of the rigidbody at rb.position
            //    Vector3 velocityPoint = (calPosition - rb.position) / Time.fixedDeltaTime * 0.5f;
            //    // Find the angular velocity and linear velocity of the COM of rigidbody.
            //    // Note: rb.velocity the velocity of center of mass, which is not always equal to
            //    // that of rb.position. Same for rb.angularVelocity. That's why we need to find
            //    // the velocity and angularVelocity for COM of the rigidbody.
            //    FollowPointMotionRb(rb, rb.position, velocityPoint, angularVelocity,
            //        _velocityMax, _distMaxForFollowingAngVel);
            //    //Vector3 velocityCM = PhysicsUtils.FindCMLinearVelFromPoint(rb, rb.transform.position, velocityPoint, angularVelocity);
            //    //rb.angularVelocity = angularVelocity;
            //    //rb.velocity = velocityCM;
            //}

            // Send the PickedUpdate event.
            OnPickedUpdateEvent(picker);
        }

        /// <summary>
        /// It will be called when this pickable object is released by the picker. It will restore
        /// the properties of the associated rigidbody.
        /// </summary>
        /// <param name="picker">The picker that releases this pickable object.</param>
        public virtual void OnReleased(IPicker picker)
        {
            if (IsPicked)
            {
                Transform pickerTransform = picker.Transform;
                Rigidbody rb = GetActiveRb();
                // Restore the properties of the associated rigidbody.
                ReleaseRbFromControlState(rb, picker);
                // Assign the correct releasing velocity and angular velocity based on the 
                // picker's velocity and angular velocity at that instance.
                FollowPointMotionRb(rb, pickerTransform.position,
                    picker.Velocity, picker.AngularVelocity,
                    _velocityMax, _distMaxForFollowingAngVel);
                // Send the PickedRelease event.
                OnReleasedEvent(picker);
            }
        }

        protected void InitPickedRb(Rigidbody rb, Transform picker)
        {
            PositionOffsetWithPicker = Quaternion.Inverse(picker.rotation) * (rb.position - picker.position);
            RotationOffsetWithPicker = Quaternion.Inverse(picker.rotation) * rb.rotation;
        }

        /// <summary>
        /// Restore the properties of the associated rigidbody when this object is released.
        /// </summary>
        /// <param name="rb">The associated rigidbody.</param>
        /// <param name="picker">The picker that releases this picable object.</param>
        protected void ReleaseRbFromControlState(Rigidbody rb, IPicker picker)
        {
            //if (GameController.Instance.AllowPassThrough)
            //{
            //    rb.isKinematic = false;
            //}
            //else
            //{
            //    rb.useGravity = true;
            //}

            rb.isKinematic = false;
            rb.constraints = _rbOriginalConstraints;
            // Unregister the relation between the picker and this rigidbody.
            PickableMapping.UpdatePickableMapping(rb, false, picker);
        }

        /// <summary>
        /// Modify the properties the associated rigidbody so it will follow the movement
        /// of the picker later.
        /// </summary>
        /// <param name="rb">The associated rigidbody.</param>
        /// <param name="picker">The picker that picks this object up.</param>
        protected void PutRbUnderControlState(Rigidbody rb, IPicker picker)
        {
            //if (GameController.Instance.AllowPassThrough)
            //{
            //    rb.isKinematic = true;
            //}
            //else
            //{
            //    rb.useGravity = false;
            //}
            rb.isKinematic = true;
            PickableMapping.UpdatePickableMapping(rb, true, picker);
            _rbOriginalConstraints = rb.constraints;
            //rb.constraints = RigidbodyConstraints.None;
        }

        /// <summary>
        /// Calculate the velocity and angular velocity of CM based on the velocity and angular
        /// velocity of the point on the rigidbody.
        /// </summary>
        /// <param name="rb">The associated rigidbody</param>
        /// <param name="pointPos">The position of the point in the world coordinate</param>
        /// <param name="pointVel">Velocity of the point</param>
        /// <param name="pointAngVel">Angular velocity of the point</param>
        /// <param name="velocityMax">Maximum velocity of the rigidbody allowed</param>
        /// <param name="distMaxForFollowingAngVel">If the distance between the point and the rigidbody
        /// is greater than this value, rigidbody's angular velocity will not be updated.</param>
        protected void FollowPointMotionRb(Rigidbody rb, Vector3 pointPos, Vector3 pointVel,
            Vector3 pointAngVel, float velocityMax, float distMaxForFollowingAngVel)
        {
            if (float.IsNaN(pointVel.magnitude) || float.IsNaN(pointAngVel.magnitude))
            {
                return;
            }
            Vector3 velocityCM = PhysicsUtils.FindCMLinearVelFromPoint(rb, pointPos, pointVel, pointAngVel);
            if (velocityCM.magnitude > velocityMax)
            {
                velocityCM = velocityCM.normalized * velocityMax;
            }
            rb.velocity = velocityCM;
            float distance = Vector3.Distance(pointPos, rb.position);
            if (distance < distMaxForFollowingAngVel)
            {
                rb.angularVelocity = pointAngVel;
            }
        }

        /// <summary>
        /// Deprecated. Will be removed in the future version.
        /// </summary>
        public void UpdateTotalMass()
        {
            float totalMass = SelfMass;
            foreach (Transform childPart in transform)
            {
                Pickable part = childPart.GetComponent<Pickable>();
                if (part != null)
                {
                    totalMass += part.TotalMass;
                }
            }
            TotalMass = totalMass;
        }

    }
}
