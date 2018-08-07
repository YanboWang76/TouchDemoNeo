/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;

namespace Libdexmo.Unity.Snapping
{
    /// <summary>
    /// This is the base class for all snappable scripts. Snappable objects are
    /// ones that snappers can be snapped to. 
    /// </summary>
    /// <remarks>
    /// Snappable objects are used to constrain motion of hand models so grasping of
    /// hand models will be more predictable and easier to control. Lever and knob
    /// game objects are attached with snappable scripts to make grasping experience
    /// better.
    /// </remarks>
    public abstract class Snappable : MonoBehaviour, ISnappable
    {
        /// <summary>
        /// Position of snapping position reference transforms are used as the target 
        /// snapping position for snappers. The first element is the reference for
        /// right hand. The second is the reference for left hand. 
        /// </summary>
        protected List<Transform> SnappingPositionReferenceList
        { get { return _snappingPositionReferenceList; } }
        /// <summary>
        /// Rotation of snapping rotation reference transforms are used as the target 
        /// snapping rotation for snappers. The first element is the reference for
        /// right hand. The second is the reference for left hand. 
        /// </summary>
        protected List<Transform> SnappingRotationReferenceList
        { get { return _snappingRotationReferenceList; } }
        /// <summary>
        /// This is a value of float that determines the minimum distance below which
        /// the transition of moving during snapping stops. 
        /// </summary>
        /// <remarks>
        /// When the collider of snappable and snapper first collides, the hand model
        /// will not immediately “teleport” to the snapping position and rotation. 
        /// Vector3.Lerp and Quaternion.Lerp are used to make the changing of position
        /// and rotation smooth. When the distance of the target snapping position and
        /// the current position of palm center is smaller than the 
        /// SnappingTransitionDistanceMin, the lerp will stop and hand model will “teleport”
        /// to the target position. If no transition is needed, simply set it to a very
        /// large value.
        /// </remarks>
        protected float SnappingTransitionDistanceMin
        { get { return _snappingTransitionDistanceMin; } }
        
        private readonly string[] _snappingPointIdentifier = { "snapping" };

        // See comment for SnappingPositionReferenceList.
        [Tooltip("The first one is default. In case of hand, first is for right hand." +
                 " The second one is for left.")]
        [SerializeField]
        private List<Transform> _snappingPositionReferenceList;
        // See comment for SnappingRotationReferenceList.
        [Tooltip("The first one is default. In case of hand, first is for right hand." +
                 " The second one is for left.")]
        [SerializeField]
        private List<Transform> _snappingRotationReferenceList;
        // See comment for SnappingTransitionDistanceMin.
        [Tooltip("This is a value that determines the minimum distance" +
                 " below which the transition of moving during snapping stops.")]
        [SerializeField]
        private float _snappingTransitionDistanceMin = 0.01f;

        /// <summary>
        /// This is called when the script is first attached or "reset" of script
        /// is clicked.
        /// </summary>
        void Reset()
        {
            FindSnappingPoint();
            FindAndChangeLayer();
        }

        /// <summary>
        /// Get the snapping position reference according to the snapper type.
        /// </summary>
        /// <remarks>
        /// Currently, snappers are for hands, but in the future, it may extend
        /// to other types, so this function can be overridden to support other
        /// types of snappers.
        /// </remarks>
        /// <param name="snapper">The snapper that is in the snapping region.</param>
        /// <returns>Transform of snapping position reference.</returns>
        public virtual Transform GetSnappingPositionReference(ISnapper snapper)
        {
            return _snappingPositionReferenceList[0];
        }

        /// <summary>
        /// Get the snapping rotation reference according to the snapper type.
        /// </summary>
        /// <remarks>
        /// Currently, snappers are for hands, but in the future, it may extend
        /// to other types, so this function can be overridden to support other
        /// types of snappers.
        /// </remarks>
        /// <param name="snapper">The snapper that is in the snapping region.</param>
        /// <returns>Transform of snapping rotation reference.</returns>
        public virtual Transform GetSnappingRotationReference(ISnapper snapper)
        {
            return _snappingRotationReferenceList[0];
        }

        /// <summary>
        /// Find layer number of "Snappable". If found, change the this game object's
        /// layer to "Snappable" layer. Game object attached with snappable script 
        /// and its children should be in "Snappable" layer, so they only collide with
        /// objects in "Snapper" layer.
        /// </summary>
        private void FindAndChangeLayer()
        {
            int snappableLayer = LayerMask.NameToLayer("Snappable");
            if (snappableLayer == -1)
            {
                Debug.LogError("Must define \"Sanppable\" layer.");
            }
            else
            {
                gameObject.layer = snappableLayer;
            }
        }

        /// <summary>
        /// Try to find the snapping reference by game object's names.
        /// </summary>
        private void FindSnappingPoint()
        {
            _snappingPositionReferenceList = new List<Transform> {null};
            _snappingRotationReferenceList = new List<Transform> {null};
            string name = transform.name.ToLower();
            if (Miscellaneous.ContainSubstringAny(name, _snappingPointIdentifier))
            {
                _snappingPositionReferenceList[0] = transform;
                _snappingRotationReferenceList[0] = transform;
                return;
            }
            foreach (Transform child in transform)
            {
                name = child.name.ToLower();
                if (Miscellaneous.ContainSubstringAny(name, _snappingPointIdentifier))
                {
                    _snappingPositionReferenceList[0] = child;
                    _snappingRotationReferenceList[0] = child;
                    return;
                }
            }
        }

        /// <summary>
        /// Check if the snapper is still moving towards the target snapping position.
        /// </summary>
        /// <remarks>
        /// The snapper is in transition when the distance between the snapper and
        /// the target snapping position is greater than _snappingTransitionDistanceMin.
        /// </remarks>
        /// <param name="snapper">The snapper that is in snapping region.</param>
        /// <returns>True if the snapper is still moving towards the target snapping
        /// position.</returns>
        public virtual bool CheckInSnappingTransition(ISnapper snapper)
        {
            Transform positionReference = GetSnappingPositionReference(snapper);
            if (positionReference == null)
            {
                return false;
            }
            SnapperDexmo snapperDexmo = snapper as SnapperDexmo;
            Vector3 snapperPosition = snapperDexmo == null
                ? snapper.Transform.position
                : snapperDexmo.PalmCenter.position;
            float dist = Vector3.Distance(positionReference.position,
                snapperPosition);
            bool inSnappingTransition = dist >= _snappingTransitionDistanceMin;
            return inSnappingTransition;
        }

        /// <summary>
        /// This function will be called when the snapper just enters the snapping
        /// region. It is empty now, but can be overridden by derived classes.
        /// </summary>
        /// <param name="snapper">The snapper in the snapping region.</param>
        public virtual void OnSnappedEnter(ISnapper snapper)
        {
        }

        /// <summary>
        /// This function will be called for every FixedUpdate cycle when the snapper
        /// is in snapping region. It replaces the position and rotation of the snapper
        /// by the target position and rotation.
        /// </summary>
        /// <remarks>
        /// Even though snapper's transform is modified here, it will not "teleport" to
        /// the target position and rotation immediately. The UnityHandController will
        /// use Vector3.Lerp and Quaternion.Lerp whenever there is a sudden change
        /// of position and rotation of the hand models, so by setting the snapper transform
        /// position and rotation to target ones, the hand controller will handle the
        /// rest of transition movement.
        /// </remarks>
        /// <param name="snapper">The snapper in the snapping transition.</param>
        public virtual void OnSnappedStay(ISnapper snapper)
        {
            Transform positionReference = GetSnappingPositionReference(snapper);
            Transform rotationReference = GetSnappingRotationReference(snapper);
            if (positionReference == null || rotationReference == null)
            {
                return;
            }
            snapper.Transform.position = positionReference.position;
            snapper.Transform.rotation = rotationReference.rotation;
        }

        /// <summary>
        /// This function is called when the snapper exits the snapping region.
        /// It is empty now, but can be overridden by derived classes.
        /// </summary>
        /// <param name="snapper">The snapper in snapping region.</param>
        public virtual void OnSnappedExit(ISnapper snapper)
        {
        }
    }
}
