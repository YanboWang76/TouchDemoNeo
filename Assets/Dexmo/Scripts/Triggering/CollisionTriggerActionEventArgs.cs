/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;

namespace Libdexmo.Unity.Triggering
{
    public class CollisionTriggerActionEventArgs: EventArgs
    {
        public ICollisionTriggeringBody CollisionTriggeringBody { get; private set; }

        public CollisionTriggerActionEventArgs(ICollisionTriggeringBody collisionTriggeringBody)
        {
            CollisionTriggeringBody = collisionTriggeringBody;
        }
    }
}
