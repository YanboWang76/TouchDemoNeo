/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Touchables;

namespace Libdexmo.Unity.Touchables
{
    /// <summary>
    /// This script gives object force feedback behaviour. When hand models are
    /// touching it from outside, there will be force feedback.
    /// </summary>
    /// <remarks>
    /// There will only be force feedback when the hand models are touching it
    /// from outside. This means the object is close to palm. There will be no
    /// force feedback when the back of hand is touching it.
    /// </remarks>
    public class TouchableStatic : MonoBehaviour, ITouchableForceFeedback
    {
        #region ITouchableForceFeedback Properties
        public GameObject BindedGameObject
        {
            get
            {
                return gameObject;
            }
        }
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
        public bool IndicateColorOnTouching
        {
            get { return _indicateColorOnTouching; }
            set { _indicateColorOnTouching = value; }
        }
        public Color IndicatedColorOnTouching
        {
            get { return _indicatedColorOnTouching; }
            set { _indicatedColorOnTouching = value; }
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
        private bool _indicateColorOnTouching = false;
        [SerializeField]
        private Color _indicatedColorOnTouching = Color.white;
        [SerializeField]
        private float _bendAngleChangedMaxAllowed = 30f;
        #endregion
    }
}
