/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Model;
using Libdexmo.Unity.Core;
using UnityEngine;

namespace Libdexmo.Unity.HandController
{
    public class HandPoseConditionManager: MonoBehaviour
    {
        public static HandPoseConditionManager Instance;

        public Dictionary<HandPoseType, HandPoseCondition> HandPoseTypeToConditionDict
        { get; private set; }

        [SerializeField]
        private List<HandPoseCondition> _handPoseConditions;

        void Reset()
        {
            _handPoseConditions = new List<HandPoseCondition>();
            HandPoseTypeToConditionDict = new Dictionary<HandPoseType, HandPoseCondition>();
            foreach (HandPoseType type in Enum.GetValues(typeof(HandPoseType)))
            {
                HandPoseCondition condition = new HandPoseCondition(type);
                _handPoseConditions.Add(condition);
                HandPoseTypeToConditionDict.Add(type, condition);
            }
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
                    DestroyImmediate(gameObject);
                }
            }
            Init();
        }

        private void Init()
        {
            HandPoseTypeToConditionDict = new Dictionary<HandPoseType, HandPoseCondition>();
            int n = _handPoseConditions.Count;
            for (int i = 0; i < n; i++)
            {
                HandPoseCondition condition = _handPoseConditions[i];
                HandPoseType type = condition.HandPoseType;
                HandPoseTypeToConditionDict.Add(type, condition);
            }
        }

        public bool CheckSatisfyCondition(HandPoseType pose, Hand handData)
        {
            return HandPoseTypeToConditionDict[pose].CheckSatisfyCondition(handData);
        }

        public bool CheckSatisfyCondition(HandPoseType pose,
            IHandRotationNormalized handRotation)
        {
            return HandPoseTypeToConditionDict[pose].CheckSatisfyCondition(handRotation);
        }
    }
}
