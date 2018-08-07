/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Core.Utility;

namespace Libdexmo.Unity.Utility
{
    public class FixedTransformFollower : MonoBehaviour
    {
        [SerializeField]
        private Transform _reference;

        private Quaternion _rotationRelativeToReference;
        private Vector3 _positionRelativeToReference;

        void Awake()
        {
            if (Miscellaneous.CheckNullAndLogError(_reference))
            {
                return;
            }
            _rotationRelativeToReference = Quaternion.Inverse(_reference.rotation) * transform.rotation;
            _positionRelativeToReference = _reference.InverseTransformPoint(transform.position);
        }

        void Update()
        {
            transform.rotation = _reference.rotation * _rotationRelativeToReference;
            transform.position = _reference.TransformPoint(_positionRelativeToReference);
        }

    }
}
