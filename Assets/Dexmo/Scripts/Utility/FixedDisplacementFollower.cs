/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections;

namespace Libdexmo.Unity.Utility
{
    public class FixedDisplacementFollower : MonoBehaviour
    {

        [SerializeField]
        private Transform _reference;

        private Vector3 _relativePositionWrtReference;

        void Start()
        {
            _relativePositionWrtReference = transform.position - _reference.position;
        }

        void FixedUpdate()
        {
            transform.position = _reference.position + _relativePositionWrtReference;
        }
    }
}
