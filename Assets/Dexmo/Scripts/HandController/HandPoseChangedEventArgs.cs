/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;

namespace Libdexmo.Unity.HandController
{
    public class HandPoseChangedEventArgs: EventArgs
    {
        public HandPoseType Pose { get; private set; }

        public HandPoseChangedEventArgs(HandPoseType pose)
        {
            Pose = pose;
        }
    }
}
