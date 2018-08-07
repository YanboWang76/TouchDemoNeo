/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// Contains picking information about a particular rigidbody.
    /// </summary>
    public class RigidbodyPickingInfo
    {
        /// <summary>
        /// Whether this rigidbody is currently picked up.
        /// </summary>
        public bool IsPicked { get; set; }
        /// <summary>
        /// If <see cref="IsPicked"/> is true, this is the picker that picks the rigidbody up.
        /// </summary>
        public IPicker Picker { get; set; }

        public RigidbodyPickingInfo()
        {
            IsPicked = false;
            Picker = null;
        }

        public RigidbodyPickingInfo(IPicker picker)
        {
            IsPicked = true;
            Picker = picker;
        }
    }
}
