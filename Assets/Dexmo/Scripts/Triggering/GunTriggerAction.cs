/******************************************************************************\
* Copyright (C) 2016 Dexta Robotics. All rights reserved.                      *
* Use subject to the terms of the Libdexmo Unity SDK Agreement at              *
* LibdexmoUnitySDKLicense.txt                                                  *
\******************************************************************************/


using UnityEngine;
using System.Collections;
using Libdexmo.Unity.Triggering;

namespace Libdexmo.Unity.Touchables.Pickables
{
    [RequireComponent(typeof(CollisionTriggeredBodyCommon))]
    public class GunTriggerAction : MonoBehaviour
    {

        [SerializeField]
        private GameObject _bullet;
        [SerializeField]
        private Transform _bulletSpawnPoint;
        [Tooltip("Bullet will fly in the forward direction")]
        [SerializeField]
        private Transform _bulletDirection;
        [SerializeField]
        [Range(0.1f, 340)]
        private float _bulletSpeed = 300;
        [SerializeField]
        private GameObject _gunfire;
        [SerializeField]
        private Transform _gunfireSpawnPoint;
        private CollisionTriggeredBodyCommon _gunTrigger;

        void Start()
        {
            _gunTrigger = GetComponent<CollisionTriggeredBodyCommon>();
            _gunTrigger.TriggerStart += GunTriggerEventHandler;
        }

        private void GunTriggerEventHandler(object sender, CollisionTriggerActionEventArgs args)
        {
            Shoot();
        }

        private void Shoot()
        {
            GameObject bullet = Instantiate(_bullet, _bulletSpawnPoint.position,
                _bulletSpawnPoint.rotation) as GameObject;
            bullet.SetActive(true);
            Rigidbody rb = bullet.AddComponent<Rigidbody>();
            rb.velocity = _bulletSpeed * _bulletDirection.forward;
            Destroy(bullet, 3);
            Instantiate(_gunfire, _gunfireSpawnPoint.position, _gunfireSpawnPoint.rotation);
        }

        void OnDestroy()
        {
            _gunTrigger.TriggerStart -= GunTriggerEventHandler;
        }
    }
}
