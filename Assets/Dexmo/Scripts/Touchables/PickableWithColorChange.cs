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
    /// This script allows a specified object to change color when this pickable object
    /// is picked up.
    /// </summary>
    public class PickableWithColorChange: Pickable
    {
        [SerializeField]
        private Color _colorOnPicked;
        [SerializeField]
        private GameObject _coloredObject;

        private Renderer _renderer;
        private Color _originalColor;

        void Reset()
        {
            _coloredObject = gameObject;
            _colorOnPicked = Color.red;
        }

        protected override void Init()
        {
            base.Init();
            _renderer = _coloredObject.GetComponent<Renderer>();
            if (_renderer == null)
            {
                Debug.LogError("Unable to find renderer.");
            }
            _originalColor = _renderer.material.color;
        }

        void Awake()
        {
            Init();
        }

        /// <summary>
        /// When this object is picked up, the specified object in _coloredObject will change
        /// to specified color in _colorOnPicked.
        /// </summary>
        /// <param name="picker">The picker that picks it up.</param>
        public override void OnPickedInit(IPicker picker)
        {
            base.OnPickedInit(picker);
            _renderer.material.color = _colorOnPicked;
        }

        /// <summary>
        /// When this object is released, the specified object in _cooredObject will change
        /// back to its original color.
        /// </summary>
        /// <param name="picker">The picker that releases it.</param>
        public override void OnReleased(IPicker picker)
        {
            base.OnReleased(picker);
            _renderer.material.color = _originalColor;
        }
    }
}
