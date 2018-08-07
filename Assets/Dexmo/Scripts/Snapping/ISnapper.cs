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
    /// ISnapper interface interacts with <see cref="ISnappable"/>. Snapper is
    /// the object that will snap itself onto other things.
    /// </summary>
    /// <remarks>
    /// Snappers are usually tracked by user input, like hands. When snappable
    /// objects are present, hand models for example can snap to them.
    /// </remarks>
    public interface ISnapper
    {
        /// <summary>
        /// Whether the snapper is currently snapped to other snappable objects.
        /// </summary>
        bool IsSnapped { get; }
        /// <summary>
        /// If <see cref="IsSnapped"/> is true, it is the snappable object that 
        /// snapper is snapped to.
        /// </summary>
        ISnappable SnappedObject { get; }
        /// <summary>
        /// Transform that has this script attached.
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        /// Update snapping information.
        /// </summary>
        void SnapUpdate();
    }
}
