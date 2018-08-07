/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;

namespace Libdexmo.Unity.Utility
{
    [Serializable]
    public struct AudioSourceField
    {
        [HideInInspector]
        public string name;
        public AudioSource Source;
    }
}
