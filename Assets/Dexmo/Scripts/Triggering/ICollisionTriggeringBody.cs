/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;

namespace Libdexmo.Unity.Triggering
{
    /// <summary>
    /// This interface interacts with <see cref="ICollisionTriggeredBody"/>. It
    /// is mainly used to be group collisions. All collisions triggered by 
    /// child colliders will be identified as a single identity. 
    /// </summary>
    /// <remarks>
    /// For example, if the hand transform is attached a script that implements
    /// this interface, all collisions triggered by finger colliders will be
    /// identified as this hand.
    /// </remarks>
    public interface ICollisionTriggeringBody
    {
    }
}
