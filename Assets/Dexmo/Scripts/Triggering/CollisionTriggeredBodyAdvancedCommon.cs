/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;

namespace Libdexmo.Unity.Triggering
{
    public class CollisionTriggeredBodyAdvancedCommon: CollisionTriggeredBodyAdvanced
    {
        void Awake()
        {
            Init();
        }

        void OnTriggerStay(Collider c)
        {
            ICollisionTriggeringBody collisionTriggeringBody = c.GetComponentInParent<ICollisionTriggeringBody>();
            if (collisionTriggeringBody != null)
            {
                OnTriggerUpdate(collisionTriggeringBody);
            }
        }
    }
}
