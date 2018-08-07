/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Model;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.HandController;
using UnityEngine;

namespace Libdexmo.Unity.HandController
{
    [Serializable]
    public class HandPoseCondition
    {
        public HandPoseType HandPoseType { get { return _handPoseType; } }
        public IHandRotationNormalizedRange HandRotationRange
        { get { return _handCondition; } }

        [SerializeField]
        private string _name;
        [SerializeField]
        private HandPoseType _handPoseType;
        [SerializeField]
        private HandRotationNormalizedCondition _handCondition;

        public HandPoseCondition(HandPoseType type)
        {
            _handPoseType = type;
            _name = type.ToString();
        }

        public bool CheckSatisfyCondition(Hand handData)
        {
            return _handCondition.CheckSatisfyCondition(handData);
        }

        public bool CheckSatisfyCondition(IHandRotationNormalized handRotation)
        {
            return _handCondition.CheckSatisfyCondition(handRotation);
        }
    }
}
