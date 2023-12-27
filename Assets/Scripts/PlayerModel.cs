using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spine.Unity.Examples.BasicPlatformerController;
using UnityEngine.Windows;
using TMPro;
using System;

namespace Game
{
    public class PlayerModel : MonoBehaviour
    {
        #region Inspector
        [Header("Current State")]
        public SpineState state;
        public bool facingLeft;
        [Range(-1f, 1f)]
        public float currentSpeed;
        public float minSpeedForIdle = 0.2f;

        [Header("Balance")]
        public float shootInterval = 0.12f;
        #endregion

        float lastShootTime;
        public event System.Action ShootEvent;  // Lets other scripts know when Spineboy is shooting. Check C# Documentation to learn more about events and delegates.
        public event System.Action StartAimEvent;   // Lets other scripts know when Spineboy is aiming.
        public event System.Action StopAimEvent;   // Lets other scripts know when Spineboy is no longer aiming.

        #region API

        public void TryShoot()
        {
            float currentTime = Time.time;

            if (currentTime - lastShootTime > shootInterval)
            {
                lastShootTime = currentTime;
                if (ShootEvent != null) ShootEvent();   // Fire the "ShootEvent" event.
            }
        }

        public void StartAim()
        {
            if (StartAimEvent != null) StartAimEvent();   // Fire the "StartAimEvent" event.
        }

        public void StopAim()
        {
            if (StopAimEvent != null) StopAimEvent();   // Fire the "StopAimEvent" event.
        }

        public void TryMove(float speed, Vector3 targetPosition)
        {
            float normalizedDistance = Mathf.InverseLerp(speed, 0.1f, Vector3.Distance(transform.position, targetPosition));
            float t = Mathf.Pow(normalizedDistance, 10);
            float localSpeed = Mathf.Lerp(speed, minSpeedForIdle, 0);
            const float SpeedThreshold = 1f;

            if (Mathf.Abs(targetPosition.x - transform.position.x) < SpeedThreshold)
            {
                localSpeed = 0;
            }

            float currentY = transform.position.y;
            float currentZ = transform.position.z;

            MoveTowardsTarget(speed, targetPosition, localSpeed, currentY, currentZ);
            UpdateFacingDirection(targetPosition, localSpeed);
            UpdateState(localSpeed);
        }

        private void MoveTowardsTarget(float speed, Vector3 targetPosition, float localSpeed, float currentY, float currentZ)
        {
            

            Vector3 newPosition = new Vector3(Vector3.MoveTowards(transform.position, targetPosition, localSpeed * Time.deltaTime).x, currentY, currentZ);
            transform.position = newPosition;
        }

        private void UpdateFacingDirection(Vector3 targetPosition, float localSpeed)
        {
            if (localSpeed != 0)
            {
                bool speedIsNegative = (targetPosition.x < transform.position.x);
                facingLeft = speedIsNegative;
            }
        }

        private void UpdateState(float localSpeed)
        {
            if (localSpeed <= 0.1)
            {
                state = SpineState.Idle;
            }
            else
            {
                state = Mathf.Abs(localSpeed) > 0.6f ? SpineState.Running : SpineState.Walk;
            }
        }
        #endregion

    }
    public enum SpineState
    {
        Idle,
        Walk,
        Running
    }
}