/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Triggering;

namespace Libdexmo.Unity.Triggering
{
    public class CollisionTriggeredBodyCommon : CollisionTriggeredBody
    {
        void Awake()
        {
            Init();
        }

        void OnTriggerEnter(Collider c)
        {
            //Debug.Log("OnTriggerEnter.");
            ICollisionTriggeringBody collisionTriggeringBody =
                c.GetComponentInParent<ICollisionTriggeringBody>();
            if (collisionTriggeringBody != null)
            {
                OnTriggerStart(collisionTriggeringBody);
            }
        }

        void OnTriggerExit(Collider c)
        {
            //Debug.Log("OnTriggerExits.");
            ICollisionTriggeringBody collisionTriggeringBody =
                c.GetComponentInParent<ICollisionTriggeringBody>();
            if (collisionTriggeringBody != null)
            {
                OnTriggerEnd(collisionTriggeringBody);
            }
        }

    }
}
