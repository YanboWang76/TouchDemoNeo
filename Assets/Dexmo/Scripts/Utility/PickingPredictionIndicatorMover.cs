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
    public class PickingPredictionIndicatorMover : MonoBehaviour
    {
        [SerializeField]
        private Transform _startPoint;
        [SerializeField]
        private Transform _endPoint;
        [SerializeField]
        [Range(0, 1)]
        private float _positionNormalized;
        [SerializeField]
        private Transform _rotationReference;
        [SerializeField]
        private Vector3 _localForwardDirection = new Vector3(1, 0, 0);

        void Awake()
        {
            if (Miscellaneous.CheckNullAndLogError(_startPoint) ||
                Miscellaneous.CheckNullAndLogError(_endPoint) ||
                Miscellaneous.CheckNullAndLogError(_rotationReference))
            {
                return;
            }
        }

        void LateUpdate()
        {
            Vector3 targetPosition = _startPoint.position + _positionNormalized * (_endPoint.position - _startPoint.position);
            Vector3 targetRight = _endPoint.position - _startPoint.position;
            Vector3 targetForward = _rotationReference.TransformDirection(_localForwardDirection);
            Vector3 targetUp = Vector3.Cross(targetForward, targetRight);
            transform.position = targetPosition;
            transform.rotation = Quaternion.LookRotation(targetForward, targetUp);
        }
    }
}
