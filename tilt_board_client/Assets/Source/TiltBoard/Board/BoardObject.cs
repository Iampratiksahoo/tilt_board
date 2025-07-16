using System;
using System.Collections.Generic;
using Source.TiltBoard.Global;
using Source.TiltBoard.Global.Signal;
using Source.TiltBoard.Signal;
using UnityEngine;

namespace Source.TiltBoard.Board
{
    public class BoardObject : MonoBehaviour
    {
        [Header("Tilt Settings")]
        [Tooltip("Max pitch (front/back) tilt in degrees.")]
        [SerializeField] private float maxPitchAngle = 15f;
        [Tooltip("Max roll (left/right) tilt in degrees.")]
        [SerializeField] private float maxRollAngle = 15f;
        [Tooltip("How fast the board interpolates to the target tilt.")]
        [SerializeField] private float tiltLerpSpeed = 10f;

        [Space]

        [Header("Corners")]
        [SerializeField] private SpawnPointGroupObject playerSpawnPointGroup = null;
        [SerializeField] private SpawnPointGroupObject opponentSpawnPointGroup = null;

        private Quaternion _targetRotation;

        /// <summary>
        /// The spawn points for the player's balls 
        /// </summary>
        public SpawnPointGroupObject PlayerSpawnPointGroup => playerSpawnPointGroup;

        /// <summary>
        /// The spawn points for the opponent's balls 
        /// </summary>
        public SpawnPointGroupObject OpponentSpawnPointGroup => opponentSpawnPointGroup;

        void Start()
        {
            _targetRotation = transform.rotation;
        }

        void Update()
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                _targetRotation,
                Time.deltaTime * tiltLerpSpeed
            );
        }

        /// <summary>
        /// Call this each frame with both players' inputs.
        /// p1Input.y pushes the near side of the board up/down (pitch).
        /// p2Input.y pushes the far  side of the board up/down (pitch).
        /// p1Input.x and p2Input.x together roll the board left/right.
        /// </summary>
        public void Tilt(Vector2 p1Input, Vector2 p2Input)
        {
            // Clamp each to –1..1
            p1Input = Vector2.ClampMagnitude(p1Input, 1f);
            p2Input = Vector2.ClampMagnitude(p2Input, 1f);

            // --- Compute pitch from two ends --- 
            // When p1 pushes up (+y), front tilts up; when p2 pushes up, back tilts up.
            // We map p1Input.y to front tilt (positive pitch) and p2Input.y to back tilt (negative pitch).
            float frontTilt = Mathf.Clamp(p1Input.y * maxPitchAngle, -maxPitchAngle, maxPitchAngle);
            float backTilt = -Mathf.Clamp(p2Input.y * maxPitchAngle, -maxPitchAngle, maxPitchAngle);
            // Average to get overall pitch angle:
            float pitch = (frontTilt + backTilt) * 0.5f;

            // --- Compute roll from horizontal inputs --- 
            // Both players pushing right (+x) rolls the board right (+Z roll).
            float rollFromP1 = Mathf.Clamp(-p1Input.x * maxRollAngle, -maxRollAngle, maxRollAngle);
            float rollFromP2 = Mathf.Clamp(-p2Input.x * maxRollAngle, -maxRollAngle, maxRollAngle);
            float roll = (rollFromP1 + rollFromP2) * 0.5f;

            // Build the target rotation (pitch around X, roll around Z):
            _targetRotation = Quaternion.Euler(pitch, 0f, roll);
        }
    }
}