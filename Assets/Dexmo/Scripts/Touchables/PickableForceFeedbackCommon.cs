/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// This class provides the most common functionality of pickable object with force feedback.
    /// Game object attached with this script will "stick" to the picker by moving to the position
    /// and rotation with initial offset with picker as kinematic rigidbody once it is grasped and
    /// restore to its original state when it is released.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PickableForceFeedbackCommon : PickableForceFeedback
    {
        void Awake()
        {
            Init();
        }
    }
}
