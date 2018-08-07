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
    /// This interface interacts with <see cref="ICollisionTriggeringBody"/>.
    /// It collects all the collisions from distinct triggering bodies and
    /// trigger TriggerStart and TriggerEnd events when there are new
    /// triggering bodies coming or exiting.
    /// </summary>
    public interface ICollisionTriggeredBody
    {
        /// <summary>
        /// This event is triggered when new triggering bodies come into contact.
        /// </summary>
        event EventHandler<CollisionTriggerActionEventArgs> TriggerStart;
        /// <summary>
        /// This event is triggered when new triggering bodies exit the contact.
        /// </summary>
        event EventHandler<CollisionTriggerActionEventArgs> TriggerEnd;
    }
}
