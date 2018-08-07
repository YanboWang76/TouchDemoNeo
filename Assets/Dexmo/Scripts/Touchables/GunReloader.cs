/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using System;
using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Touchables.Pickables;
using Libdexmo.Unity.Triggering;

namespace Libdexmo.Unity.Touchables.Pickables
{
    [RequireComponent(typeof(LinearSliderWithBoundaryTriggerControllerForceFeedback))]
    public class GunReloader : MonoBehaviour
    {

        [SerializeField]
        private GameObject _bulletShell;
        [SerializeField]
        private Transform _bulletShellEjectionReference;
        [Tooltip("Bullet shell will be ejected in the forward direction of this transform")]
        [SerializeField]
        private Transform _bulletShellEjectionDirection;
        [SerializeField]
        private float _ejectionSpeed = 0.5f;

        private LinearSliderWithBoundaryTriggerControllerForceFeedback _linearSlider;

        void Start()
        {
            _linearSlider =
                GetComponent<LinearSliderWithBoundaryTriggerControllerForceFeedback>();
            _linearSlider.TriggerStatusChanged += OnTriggerStatusChanged;
        }

        private void OnTriggerStatusChanged(object sender, TwoBoundaryTriggerEventArgs args)
        {
            TwoBoundaryTriggerState triggerState = args.TriggerState;
            if (triggerState == TwoBoundaryTriggerState.End)
            {
                EjectBulletShell();
            }
        }

        private void EjectBulletShell()
        {
            GameObject bulletShell = Instantiate(_bulletShell,
                _bulletShellEjectionReference.position,
                _bulletShellEjectionReference.rotation)
                as GameObject;
            bulletShell.SetActive(true);
            Rigidbody rb = bulletShell.AddComponent<Rigidbody>();
            rb.mass = 0.1f;
            rb.velocity = _ejectionSpeed * _bulletShellEjectionDirection.forward;
            Destroy(bulletShell, 3);
        }

        void OnDestroy()
        {
            _linearSlider.TriggerStatusChanged -= OnTriggerStatusChanged;
        }
    }
}
