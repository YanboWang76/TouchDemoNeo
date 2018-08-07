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
    public interface ITwoBoundaryTrigger
    {
        event EventHandler<TwoBoundaryTriggerEventArgs> TriggerStatusChanged;
        TwoBoundaryTriggerState TriggerState { get; }
    }
}
