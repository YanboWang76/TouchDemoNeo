/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Pickers;
using UnityEngine;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// This static class maintains a global dictionary that keeps the relationship between any rigidbody
    /// and its picker.
    /// </summary>
    /// <remarks>
    /// This class is needed to handle cases where pickable objects are of hierarchical relationship and rigidbodies
    /// are created in the runtime. For example, if pickable object A has a child B which is also attached with a pickable
    /// script. Initially A has a rigidbody and B does not, since B is the child of A. At some point in time in the
    /// game, B may no longer be the child of A and then B has a rigidbody attached at the runtime. If all of this
    /// occurs when A is still held by the picker, sometimes undesired behaviour may happen, and this class helps
    /// to resolve those issues.
    /// </remarks>
    public static class PickableMapping
    {
        // The dictionary that keeps the relationship between rigidbody and its picking information
        private static Dictionary<Rigidbody, RigidbodyPickingInfo> _pickableRbToPickingInfoDict =
            new Dictionary<Rigidbody, RigidbodyPickingInfo>();

        /// <summary>
        /// Return the picking information about the rigidbody.
        /// </summary>
        /// <param name="rb">Rigidbody to check.</param>
        /// <returns>Picking information of the rigidbody. Null if the rigidbody has
        /// not been registered before.</returns>
        public static RigidbodyPickingInfo GetPickingInfo(Rigidbody rb)
        {
            RigidbodyPickingInfo pickerInfo;
            if (_pickableRbToPickingInfoDict.TryGetValue(rb, out pickerInfo))
            {
                return pickerInfo;
            }
            return null;
        }

        /// <summary>
        /// Check if the rigidbody is currently being held by any picker.
        /// </summary>
        /// <param name="rb">The rigidbody to check.</param>
        /// <returns>True if the rigidbody is picked up.</returns>
        public static bool CheckIsPicked(Rigidbody rb)
        {
            RigidbodyPickingInfo pickerInfo = GetPickingInfo(rb);
            if (pickerInfo == null)
            {
                return false;
            }
            else
            {
                return pickerInfo.IsPicked;
            }
        }

        /// <summary>
        /// Return the picker if the rigidbody is picked up.
        /// </summary>
        /// <param name="rb">Rigidbody to check.</param>
        /// <returns>Picker of the rigidbody if any. Null if the rigidbody
        /// is not picked up.</returns>
        public static IPicker GetAssociatedPicker(Rigidbody rb)
        {
            RigidbodyPickingInfo pickerInfo = GetPickingInfo(rb);
            if (pickerInfo == null)
            {
                return null;
            }
            else
            {
                return pickerInfo.Picker;
            }
        }

        /// <summary>
        /// Update the dictionary that keeps rigidbodies and their picking information. This should
        /// be called whenever a pickable object is picked up or released.
        /// </summary>
        /// <param name="pickableRb">Rigidbody of the pickable object.</param>
        /// <param name="isPicked">Whether this rigidbody is picked up.</param>
        /// <param name="picker">The picker of the rigidbody.</param>
        public static void UpdatePickableMapping(Rigidbody pickableRb, bool isPicked, IPicker picker)
        {
            if (picker == null)
            {
                throw new ArgumentNullException("picker");
            }
            RigidbodyPickingInfo pickingInfo;
            if (!_pickableRbToPickingInfoDict.TryGetValue(pickableRb, out pickingInfo))
            {
                // If the rigidbody is not in the dictionary, create new picking info for this one.
                pickingInfo = new RigidbodyPickingInfo();
                _pickableRbToPickingInfoDict.Add(pickableRb, pickingInfo);
            }
            if (isPicked)
            {
                // The picker picks up this object
                pickingInfo.Picker = picker;
                pickingInfo.IsPicked = true;
            }
            else
            {
                if (picker == pickingInfo.Picker)
                {
                    // Current picker is releasing this pickable object
                    pickingInfo.IsPicked = false;
                    pickingInfo.Picker = null;
                }
                else
                {
                    // This should never happen
                }
            }
        }
    }
}
