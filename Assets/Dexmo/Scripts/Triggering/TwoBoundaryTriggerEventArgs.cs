/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Touchables.Pickables;

namespace Libdexmo.Unity.Triggering
{
    public class TwoBoundaryTriggerEventArgs: EventArgs
    {
        public TwoBoundaryTriggerState TriggerState { get; private set; }

        public TwoBoundaryTriggerEventArgs(TwoBoundaryTriggerState state)
        {
            TriggerState = state;
        }
    }
}
