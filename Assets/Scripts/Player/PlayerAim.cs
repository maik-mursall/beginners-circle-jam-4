﻿using System;
using Combat;
using UnityEngine;

namespace Player
{
    public class PlayerAim : MonoBehaviour
    {
        private Camera _mainCamera;

        [SerializeField] private WeaponHandler weaponHandler;
        [SerializeField] private float turnSpeed = 8f;
        private float _currentTurnSpeed;

        public void SetCurrentTurnSpeed(float newSpeed) => _currentTurnSpeed = newSpeed;
        public void ResetCurrentTurnSpeed() => _currentTurnSpeed = turnSpeed;

        private void Awake()
        {
            ResetCurrentTurnSpeed();
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        void Update()
        {
            if (!weaponHandler.PlayerCanTurn) return;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        
            Plane plane = new Plane(Vector3.up, Vector3.up * transform.position.y);
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 target = ray.GetPoint(distance);
                Vector3 direction = target - transform.position;
                float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotation, 0), _currentTurnSpeed * Time.deltaTime);
            }
        }
    }
}