/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections;

namespace Libdexmo.Unity.Pickers
{
    /// <summary>
    /// This class provides functionality for "hand skeleton", which can be followed by "hand skin".
    /// "Hand skeleton" is tracked in reality, and "hand skin" is the graphics hand model that
    /// will move to the skeleton's position. This mechanism has not been fully tested yet.
    /// </summary>
    public class PickerHandSkeleton : Picker
    {
        public Transform HandSkin;

        protected override void Start()
        {
            base.Start();
            AttachPickedObj(HandSkin);
        }

        void FixedUpdate()
        {
            PickingFixedUpdate();
        }
    }
}
