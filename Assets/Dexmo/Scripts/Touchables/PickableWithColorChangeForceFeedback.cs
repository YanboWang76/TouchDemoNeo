/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using UnityEngine;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// This script allows a specified object to change color with force feedback
    /// when this object is picked up. 
    /// </summary>
    public class PickableWithColorChangeForceFeedback: 
        PickableWithColorChange, IPickableForceFeedback
    {
        public float Stiffness
        {
            get { return _stiffness; }
            set { _stiffness = value; }
        }
        public bool ConstrainFingerOnTouching
        {
            get { return _constrainFingerOnTouching; }
            set { _constrainFingerOnTouching = value; }
        }
        public float BendAngleChangedMaxAllowed
        {
            get { return _bendAngleChangedMaxAllowed; }
            set { _bendAngleChangedMaxAllowed = value; }
        }
        [SerializeField]
        private float _stiffness = 1;
        [SerializeField]
        private bool _constrainFingerOnTouching = true;
        [SerializeField]
        private float _bendAngleChangedMaxAllowed = 30f;

    }
}
