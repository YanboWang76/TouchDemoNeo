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
    public abstract class TriggeringObjectManagerAdvanced<T>: MonoBehaviour
        where T: class
    {
        protected HashSet<T> TriggeringObjectSet { get; private set; }

        protected virtual void Awake()
        {
            TriggeringObjectSet = new HashSet<T>();
        }

        protected virtual void FixedUpdate()
        {
            UpdateTriggeringObject();
        }

        protected virtual void UpdateTriggeringObject()
        {
            TriggeringObjectSet.Clear();
        }

        protected virtual void OnTriggerStay(Collider c)
        {
            T triggeringObject = c as T;
            if (triggeringObject != null)
            {
                TriggeringObjectSet.Add(triggeringObject);
            }
            else
            {
                triggeringObject = c.GetComponentInParent<T>();
                if (triggeringObject != null)
                {
                    TriggeringObjectSet.Add(triggeringObject);
                }
            }
        }

        public HashSet<T> GetTriggeringObject()
        {
            return TriggeringObjectSet;
        } 
    }
}
