/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core.Utility;

namespace Libdexmo.Unity.Triggering 
{
    public abstract class TriggeringObjectManagerAdvancedWithEvent<T> :
        TriggeringObjectManagerAdvanced<T> where T : class
    {
        public event EventHandler<GenericEventArgs<T>> TriggerStart;
        public event EventHandler<GenericEventArgs<T>> TriggerEnd;

        protected HashSet<T> TriggeringObjectSetLast { get; private set; }
        protected HashSet<T> TriggeringObjectEntered { get; private set; }
        protected HashSet<T> TriggeringObjectExited { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            TriggeringObjectSetLast = new HashSet<T>();
            TriggeringObjectEntered = new HashSet<T>();
            TriggeringObjectExited = new HashSet<T>();
        }

        protected override void UpdateTriggeringObject()
        {
            Miscellaneous.FindHashSetDifference(
                TriggeringObjectSet, TriggeringObjectSetLast,
                TriggeringObjectEntered, TriggeringObjectExited);
            foreach (T objectEntered in TriggeringObjectEntered)
            {
                OnTriggerStart(objectEntered);
            }

            foreach (T objectExited in TriggeringObjectExited)
            {
                OnTriggerEnd(objectExited);
            }
            TriggeringObjectSetLast.Clear();
            TriggeringObjectSetLast.CopyFrom(TriggeringObjectSet);
            TriggeringObjectSet.Clear();
        }

        protected void OnTriggerStart(T triggeringObject)
        {
            GenericEventArgs<T> args =
                new GenericEventArgs<T>(triggeringObject);
            Miscellaneous.InvokeEvent(TriggerStart, this, args);
        }

        protected void OnTriggerEnd(T triggeringObject)
        {
            GenericEventArgs<T> args =
                new GenericEventArgs<T> (triggeringObject);
            Miscellaneous.InvokeEvent(TriggerEnd, this, args);
        }
    }
}
