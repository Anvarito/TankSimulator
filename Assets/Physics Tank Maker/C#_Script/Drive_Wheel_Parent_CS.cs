using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ChobiAssets.PTM
{

    public class Drive_Wheel_Parent_CS : MonoBehaviour
    {
        /*
		 * This script is attached to "Create_###Wheels" objects that has drive wheels under the hierarchy.
		 * This script controls the angular velocity, angular drug and torque of the child drive wheels.
		 * This script works in combination with "Drive_Control_CS" in the MainBody, and "Drive_Wheel_CS" in the child drive wheels.
		*/

        // User options >>
        public bool Drive_Flag = true;
        public float Radius = 0.3f;
        public bool Use_BrakeTurn = true;
        // << User options


        // Referred to from "Drive_Wheel_CS".
        private float Max_Angular_Velocity;
        private float Left_Angular_Drag;
        private float Right_Angular_Drag;
        private float Left_Torque;
        private float Right_Torque;

        private bool _isRightBroken;
        private bool _isLeftBroken;

        float maxAngVelocity;
        Drive_Control_CS controlScript;
        private List<Drive_Wheel_CS> _driveWheels = new List<Drive_Wheel_CS>();
        private DamageManager _damageManager;

        void Start()
        {
            Initialize();
        }


        void Initialize()
        {
            // Get the "Drive_Control_CS".
            controlScript = GetComponentInParent<Drive_Control_CS>();
            _damageManager = GetComponentInParent<DamageManager>();

            foreach(Transform driveWheel in transform)
            {
                if(driveWheel.TryGetComponent(out Drive_Wheel_CS drive_Wheel))
                {
                    _driveWheels.Add(drive_Wheel);
                }
            }
            // Set the "maxAngVelocity".
            maxAngVelocity = Mathf.Deg2Rad * ((controlScript.Max_Speed / (2.0f * Radius * Mathf.PI)) * 360.0f);

            _damageManager.OnTrackBreach.AddListener(TrackBreach);
            _damageManager.OnTrackRestore.AddListener(TrackRestore);
        }

        private void TrackRestore(TrackInfoHolder track)
        {
            if (track.IsRightSide)
                _isRightBroken = false;
            else
                _isLeftBroken = false;

            foreach(var i in _driveWheels)
            {
                i.Track_Repaired_Linkage(!track.IsRightSide);
            }
        }

        private void TrackBreach(TrackInfoHolder track)
        {
            if (track.IsRightSide)
                _isRightBroken = true;
            else
                _isLeftBroken = true;

            foreach (var i in _driveWheels)
            {
                i.Track_Destroyed_Linkage(!track.IsRightSide);
            }
        }

        void Update()
        {
            Control_Velocity_And_Torque();
        }


        void Control_Velocity_And_Torque()
        {
            // Set the "Max_Angular_Velocity".
            Max_Angular_Velocity = controlScript.Speed_Rate * maxAngVelocity;

            // Set the brake drag.
            if (Use_BrakeTurn)
            {
                Left_Angular_Drag = controlScript.L_Brake_Drag;
                Right_Angular_Drag = controlScript.R_Brake_Drag;
            }
            else
            {
                Left_Angular_Drag = 0.0f;
                Right_Angular_Drag = 0.0f;
            }

            // Set the torque.
            if (Drive_Flag == true)
            {
                Left_Torque = controlScript.Left_Torque;
                Right_Torque = controlScript.Right_Torque;
            }
            else
            {
                Left_Torque = 0.0f;
                Right_Torque = 0.0f;
            }
        }

        void FixedUpdate()
        {
            Control_Rigidbody();
        }

        

        void Control_Rigidbody()
        {
            // Set the "maxAngularVelocity" of the rigidbody.
            for (int i = 0; i < _driveWheels.Count; i++)
            {
                if (_driveWheels[i].IsLeft() && _isLeftBroken
                    || !_driveWheels[i].IsLeft() && _isRightBroken)
                    continue;


                Rigidbody This_Rigidbody = _driveWheels[i].GetRigidbody();
                This_Rigidbody.maxAngularVelocity = Max_Angular_Velocity;

                // Set the "angularDrag" of the rigidbody, and add torque to it.
                if (_driveWheels[i].IsLeft())
                { // Left
                    This_Rigidbody.angularDrag = Left_Angular_Drag;
                    This_Rigidbody.AddRelativeTorque(0.0f, Left_Torque, 0.0f);
                }
                else
                { // Right
                    This_Rigidbody.angularDrag = Right_Angular_Drag;
                    This_Rigidbody.AddRelativeTorque(0.0f, Right_Torque, 0.0f);
                }
            }
        }

        void MainBody_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS".
            Destroy(this);
        }


        void Pause(bool isPaused)
        { // Called from "Game_Controller_CS".
            if (controlScript)
            {
                this.enabled = !isPaused;
            }
        }

    }

}