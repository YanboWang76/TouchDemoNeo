/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections;

namespace Libdexmo.Unity.Snapping
{
    /// <summary>
    /// ISnappable interface interacts <see cref="ISnapper"/>. Snappable is the object
    /// that snapper can be snapped to. 
    /// </summary>
    /// <remarks>
    /// Snappable objects are used to constrain motion of hand models so grasping of
    /// hand models will be more predictable and easier to control. Lever and knob
    /// game objects are attached with snappable scripts to make grasping experience
    /// better.
    /// </remarks>
    public interface ISnappable
    {
        /// <summary>
        /// This function will be called when the snapping just occurs, i.e. when
        /// snapper enters the snapping region.
        /// </summary>
        /// <param name="snapper">The snapper that is in the snapping region.</param>
        void OnSnappedEnter(ISnapper snapper);
        /// <summary>
        /// This function will be called at every FixedUpdate cycle to update
        /// snapping information as long as the snapper is in the snapping region.
        /// </summary>
        /// <param name="snapper">The snapper that is in the snapping region.</param>
        void OnSnappedStay(ISnapper snapper);
        /// <summary>
        /// This function will be called when snapper exits the snapping region.
        /// </summary>
        /// <param name="snapper">The snapper that exits the snapping region.</param>
        void OnSnappedExit(ISnapper snapper);
        /// <summary>
        /// Check if the snapper is still moving to the snapping position. 
        /// </summary>
        /// <remarks>
        /// When snapper enters the snapping region, it will be snapped to the
        /// target position and rotation, but this occurs gradually, so this 
        /// function checks if it is still moving towards the target position.
        /// </remarks>
        /// <param name="snapper">The snapper that is in the snapping region.</param>
        /// <returns>True if snapper is still moving towards the target position.</returns>
        bool CheckInSnappingTransition(ISnapper snapper);
        /// <summary>
        /// Get the "snapping position reference" transform. The target snapping 
        /// position of snapper is determined by the position of 
        /// "snapping position reference" transform.
        /// </summary>
        /// <param name="snapper">The snapper that is in the snapping region.</param>
        /// <returns>Snapping position reference transform.</returns>
        Transform GetSnappingPositionReference(ISnapper snapper);
        /// <summary>
        /// Get the "snapping rotation reference" transform. The target snapping
        /// rotation of snapper is determined by the rotation of "snapping rotation
        /// reference" transform.
        /// </summary>
        /// <param name="snapper">The snapper that is in the snapping region.</param>
        /// <returns>Snapping rotation reference transform.</returns>
        Transform GetSnappingRotationReference(ISnapper snapper);
    }
}
