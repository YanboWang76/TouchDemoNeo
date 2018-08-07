/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;

namespace Libdexmo.Unity.Triggering
{
    public class AnalogTriggerEventArgs: EventArgs
    {
        public float AnalogValue { get; private set; }

        public AnalogTriggerEventArgs(float analogValue)
        {
            AnalogValue = analogValue;
        }
    }
}
