/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;

namespace Libdexmo.Unity.Touchables.Pickables
{
    /// <summary>
    /// This abstract class is the base class of common pickable object with force feedback.
    /// </summary>
    public abstract class PickableForceFeedback : Pickable, IPickableForceFeedback
    {
        #region IPickableForceFeedback Implementation

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

        #endregion
    }
}
