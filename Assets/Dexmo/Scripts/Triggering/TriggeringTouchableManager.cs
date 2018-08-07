/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using UnityEngine;

namespace Libdexmo.Unity.Triggering
{
    [RequireComponent(typeof(Rigidbody))]
    public class TriggeringTouchableManager:
        TriggeringObjectManagerAdvanced<ITouchableForceFeedback>
    {
    }
}
