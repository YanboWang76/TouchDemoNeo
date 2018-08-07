/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using System.Collections.Generic;
using Libdexmo.Unity.Core;
using Libdexmo.Unity.Core.Utility;
using Libdexmo.Unity.Touchables.Pickables;
using UnityEngine;

namespace Libdexmo.Unity.Touchables
{
    /// <summary>
    /// This script allows gives the object two layers of stiffness. It provides
    /// public methods to change stiffness and must be used together with other
    /// scripts.
    /// </summary>
    /// <remarks>
    /// This object should be attached with primitive colliders. There are two
    /// stages, "soft" and "hard". When the stage switches to another, the stiffness
    /// will change accordingly. Position of this object can also be changed at
    /// the same time, so the contact surface for force feedback will change. This
    /// can be used in button pressing for example. When button is pressed, the
    /// stiffness is soft. When the button is pressed all the way down, the stiffness
    /// will become hard and the contact surface will change to the button down
    /// position. 
    /// </remarks>
    [RequireComponent(typeof(Rigidbody))]
    public class TwoStagePositionChangeTouchableForceFeedback: MonoBehaviour,
        ITwoStageTouchableForceFeedback
    {
        #region ITwoStageTouchableForceFeedback Properties
        public float Stiffness
        {
            get { return _stiffness; }
            set { _stiffness = value; }
        }
        public float StiffnessSoft
        {
            get { return _stiffnessSoft; }
            set { _stiffnessSoft = value; }
        }
        public float StiffnessHard
        {
            get { return _stiffnessHard; }
            set { _stiffnessHard = value; }
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
        public GameObject BindedGameObject { get { return gameObject; } }
        #endregion

        [Tooltip("Stiffness used in \"soft\" stage.")]
        [SerializeField]
        private float _stiffnessSoft = 0.1f;
        [Tooltip("Stiffness used in \"hard\" stage.")]
        [SerializeField]
        private float _stiffnessHard = 2f;
        [SerializeField]
        private bool _constrainFingerOnTouching = false;
        [SerializeField]
        private bool _indicateColorOnTouching = false;
        [SerializeField]
        private Color _indicatedColorOnTouching = Color.white;
        [SerializeField]
        private float _bendAngleChangedMaxAllowed = 90f;
        [Tooltip("Position of this object in \"soft\" stage.")]
        [SerializeField]
        private Transform _startPositionReference;
        [Tooltip("Position of this object in \"hard\" stage.")]
        [SerializeField]
        private Transform _endPositionReference;

        private readonly string[] _startPositionReferenceIdentifier = { "StartPositionReference" };
        private readonly string[] _endPositionReferenceIdentifier = { "EndPositionReference" };

        private TwoStageForceFeedbackState _state;
        private float _stiffness;
        private Rigidbody _rb;

        void Reset()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
            _rb.isKinematic = true;
            _startPositionReference =
                transform.parent.GetComponentInChildrenContainingNames<Transform>(
                    _startPositionReferenceIdentifier);
            _endPositionReference =
                transform.parent.GetComponentInChildrenContainingNames<Transform>(
                    _endPositionReferenceIdentifier);
        }

        void Awake()
        {
            _state = TwoStageForceFeedbackState.Soft;
            UpdateTouchable();
        }

        /// <summary>
        /// Update the stage of stiffness and position.
        /// </summary>
        private void UpdateTouchable()
        {
            switch (_state)
            {
                case TwoStageForceFeedbackState.Soft:
                    _stiffness = _stiffnessSoft;
                    _constrainFingerOnTouching = false;
                    transform.position = _startPositionReference.position;
                    break;

                case TwoStageForceFeedbackState.Hard:
                    _stiffness = _stiffnessHard;
                    _constrainFingerOnTouching = true;
                    transform.position = _endPositionReference.position;
                    break;
            }
        }

        /// <summary>
        /// Update the force feedback state from the new state and change stiffness
        /// and position if needed.
        /// </summary>
        /// <param name="newState">The current new state of force feedback.</param>
        public void UpdateForceFeedbackState(TwoStageForceFeedbackState newState)
        {
            switch (_state)
            {
                case TwoStageForceFeedbackState.Soft:
                    if (newState == TwoStageForceFeedbackState.Hard)
                    {
                        _state = TwoStageForceFeedbackState.Hard;
                        UpdateTouchable();
                    }
                    break;

                case TwoStageForceFeedbackState.Hard:
                    if (newState == TwoStageForceFeedbackState.Soft)
                    {
                        _state = TwoStageForceFeedbackState.Soft;
                        UpdateTouchable();
                    }
                    break;
            }
        }
    }
}
