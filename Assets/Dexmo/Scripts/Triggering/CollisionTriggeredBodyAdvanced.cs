/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Triggering;
using UnityEngine;

namespace Libdexmo.Unity.Triggering
{
    /// <summary>
    /// For this advanced version, it can perform collision checking properly even if
    /// CollisionTriggeringBody is destroyed while still inside the trigger, but this is
    /// more computationally expensive.
    /// </summary>
    public class CollisionTriggeredBodyAdvanced: MonoBehaviour, ICollisionTriggeredBody
    {
        public event EventHandler<CollisionTriggerActionEventArgs> TriggerStart;
        public event EventHandler<CollisionTriggerActionEventArgs> TriggerEnd;

        private HashSet<ICollisionTriggeringBody> _triggeringBodySetLast;
        private HashSet<ICollisionTriggeringBody> _triggeringBodySet;
        private HashSet<ICollisionTriggeringBody> _triggeringBodyEntered;
        private HashSet<ICollisionTriggeringBody> _triggeringBodyExited;

        protected virtual void Init()
        {
            _triggeringBodySet = new HashSet<ICollisionTriggeringBody>();
            _triggeringBodySetLast = new HashSet<ICollisionTriggeringBody>();
            _triggeringBodyEntered = new HashSet<ICollisionTriggeringBody>();
            _triggeringBodyExited = new HashSet<ICollisionTriggeringBody>();
        }

        protected virtual void FixedUpdate()
        {
            UpdateTriggeringStatus();
        }

        protected void OnTriggerUpdate(ICollisionTriggeringBody collisionTriggeringBody)
        {
            _triggeringBodySet.Add(collisionTriggeringBody);
        }

        private void UpdateTriggeringStatus()
        {
            Miscellaneous.FindHashSetDifference(_triggeringBodySet, _triggeringBodySetLast,
                _triggeringBodyEntered, _triggeringBodyExited);
            foreach (ICollisionTriggeringBody bodyEntered in _triggeringBodyEntered)
            {
                OnTriggerStart(bodyEntered);
            }
            foreach (ICollisionTriggeringBody bodyExited in _triggeringBodyExited)
            {
                OnTriggerEnd(bodyExited);
            }
            _triggeringBodySetLast.Clear();
            _triggeringBodySetLast.CopyFrom(_triggeringBodySet);
            _triggeringBodySet.Clear();
        }

        protected void OnTriggerStart(ICollisionTriggeringBody triggeringBody)
        {
            CollisionTriggerActionEventArgs args = new CollisionTriggerActionEventArgs(triggeringBody);
            Miscellaneous.InvokeEvent(TriggerStart, this, args);
        }

        protected void OnTriggerEnd(ICollisionTriggeringBody triggeringBody)
        {
            CollisionTriggerActionEventArgs args = new CollisionTriggerActionEventArgs(triggeringBody);
            Miscellaneous.InvokeEvent(TriggerEnd, this, args);
        }
    }
}
