/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using UnityEngine;
using System.Collections;

namespace Libdexmo.Unity.Triggering
{
    public interface IAnalogTrigger
    {
        event EventHandler<AnalogTriggerEventArgs> TriggerStatusChanged; 
        float AnalogValue { get; }
    }
}
