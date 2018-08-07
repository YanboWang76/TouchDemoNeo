/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;

namespace Libdexmo.Unity.Triggering
{
    public abstract class CollisionTriggeredBody : MonoBehaviour, ICollisionTriggeredBody
    {
        public event EventHandler<CollisionTriggerActionEventArgs> TriggerStart;
        public event EventHandler<CollisionTriggerActionEventArgs> TriggerEnd;

        private Dictionary<ICollisionTriggeringBody, int> _triggeringBodyToRefCountDict;

        protected virtual void Init()
        {
            _triggeringBodyToRefCountDict = new Dictionary<ICollisionTriggeringBody, int>();
        }

        protected void OnTriggerStart(ICollisionTriggeringBody collisionTriggeringBody)
        {
            int refCountBeforeIncrement;
            if (_triggeringBodyToRefCountDict.TryGetValue(collisionTriggeringBody, out refCountBeforeIncrement))
            {
                _triggeringBodyToRefCountDict[collisionTriggeringBody]++;
            }
            else
            {
                _triggeringBodyToRefCountDict.Add(collisionTriggeringBody, 1);
            }
            if (refCountBeforeIncrement == 0)
            {
                CollisionTriggerActionEventArgs args = new CollisionTriggerActionEventArgs(collisionTriggeringBody);
                Miscellaneous.InvokeEvent<CollisionTriggerActionEventArgs>(TriggerStart,
                    this, args);
            }
        }

        protected void OnTriggerEnd(ICollisionTriggeringBody collisionTriggeringBody)
        {
            int refCountBeforeDecrement;
            if (!_triggeringBodyToRefCountDict.TryGetValue(collisionTriggeringBody,
                    out refCountBeforeDecrement))
            {
                Debug.LogErrorFormat("Unable to find triggering body key. {0}",
                    collisionTriggeringBody);
                return;
            }
            if (refCountBeforeDecrement == 1)
            {
                _triggeringBodyToRefCountDict.Remove(collisionTriggeringBody);
                CollisionTriggerActionEventArgs args = new CollisionTriggerActionEventArgs(collisionTriggeringBody);
                Miscellaneous.InvokeEvent<CollisionTriggerActionEventArgs>(TriggerEnd,
                    this, args);
            }
            else
            {
                _triggeringBodyToRefCountDict[collisionTriggeringBody]--;
            }
        }
    }
}
