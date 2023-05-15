﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

    public class Cannon_Fire_CS : MonoBehaviour
    {
        /*
		 * This script is attached to the "Cannon_Base" in the tank.
		 * This script controls the firining of the tank.
		 * When firing, this script calls "Bullet_Generator_CS" and "Recoil_Brake_CS" scripts placed under this object in the hierarchy.
		 * In case of AI tank, this script works in combination with "AI_CS", "Turret_Horizontal_CS", "Cannon_Vertical_CS" and "Aiming_Control_CS".
		*/
        [SerializeField] private Recoil_Brake_CS _recoil_Brake_CS;
        [SerializeField] private Bullet_Generator_CS _bullet_Generator_Script;
        public Bullet_Generator_CS Bullet_Generator_Script => _bullet_Generator_Script;
        // User options >>
        public float Reload_Time = 2.0f;
        public float Recoil_Force = 5000.0f;
        // << User options


        // Set by "inputType_Settings_CS".
        public int inputType = 0;

        // Referred to from "UI_Reloading_Circle_CS".
        public float Loading_Count;

        Rigidbody bodyRigidbody;
        Transform thisTransform;
        int direction = 1; // For twin barrels, 1 = left, 2 = right.

        protected Cannon_Fire_Input_00_Base_CS inputScript;

        private bool _isSelected;
        private IEnumerator _reloadRoutine;
        private bool _IsLoaded = true;

        public UnityEvent OnReload;
        public UnityEvent OnEndReload;
        public UnityEvent<Vector3, Vector3> OnRecoil;
        private bool _isDestroyTank;


        public void Initialize(Cannon_Fire_Input_00_Base_CS canonFireInputType)
        { // This function must be called in Start() after changing the hierarchy.
            thisTransform = transform;

            if (_recoil_Brake_CS) _recoil_Brake_CS.Initialize();
            else Debug.LogError("Recoil_Brake_CS not assigned");

            if (_bullet_Generator_Script) _bullet_Generator_Script.Initialize();
            else Debug.LogError("Bullet_Generator_CS not assigned");


            bodyRigidbody = GetComponentInParent<Rigidbody>();

            // Get the input type.
            if (inputType != 10)
            { // This tank is not an AI tank.
                inputType = General_Settings_CS.Input_Type;
            }

            //DUMB solved


            // Set the "inputScript".
            inputScript = canonFireInputType;

            // Prepare the "inputScript".
            if (inputScript != null)
            {
                inputScript.Prepare(this);
            }

        }

        //protected virtual void Set_Input_Script(int type)
        //{
        //    switch (type)
        //    {
        //        case 0: // Mouse + Keyboard (Stepwise)
        //        case 1: // Mouse + Keyboard (Pressing)
        //            inputScript = gameObject.AddComponent<Cannon_Fire_Input_01_Mouse_CS>();
        //            break;

        //        case 2: // GamePad (Single stick)
        //        case 3: // GamePad (Twin stick)
        //        case 4: // GamePad (Triggers)
        //            inputScript = gameObject.AddComponent<Cannon_Fire_Input_02_For_Sticks_Drive_CS>();
        //            break;

        //        case 10: // AI
        //            inputScript = gameObject.AddComponent<Cannon_Fire_Input_99_AI_CS>();
        //            break;
        //    }
        //}


        void Update()
        {
            if (!_IsLoaded || _isDestroyTank)
            {
                return;
            }

            if (_isSelected || inputType == 10)
            { // The tank is selected, or AI.
                inputScript.Get_Input();
            }
        }


        public void Fire()
        { // Called from "Cannon_Fire_Input_##_###".
            // Call all the "Bullet_Generator_CS".

            if (!_IsLoaded)
                return;

            _bullet_Generator_Script.Fire_Linkage(direction);

            // Call all the "Recoil_Brake_CS".
            _recoil_Brake_CS.Fire_Linkage(direction);


            // Add recoil shock force to the MainBody.
            OnRecoil?.Invoke(-thisTransform.forward * Recoil_Force, thisTransform.position);

            // Reload.
            Reload();
        }

        public void SwitchBulletType()
        {
            _bullet_Generator_Script.Switch_Bullet_Type();
            Reload();
        }

        public void Reload()
        {
            if (_reloadRoutine != null)
                StopCoroutine(_reloadRoutine);

            _reloadRoutine = ReloadRoutine();
            StartCoroutine(_reloadRoutine);

            OnReload?.Invoke();
        }
        private IEnumerator ReloadRoutine()
        { // Called also from "Cannon_Fire_Input_##_###".
            _IsLoaded = false;
            Loading_Count = 0.0f;

            while (Loading_Count < Reload_Time)
            {
                Loading_Count += Time.deltaTime;
                yield return null;
            }


            OnEndReload?.Invoke();
            _IsLoaded = true;
            Loading_Count = Reload_Time;

            // Set direction for twin barrels, 1 = left, 2 = right.
            if (direction == 1)
            {
                direction = 2;
            }
            else
            {
                direction = 1;
            }
        }


        void Get_AI_CS()
        { // Called from "AI_CS".
            inputType = 10;
        }


        void Selected(bool isSelected)
        { // Called from "ID_Settings_CS".
            this._isSelected = isSelected;
        }


        public void TankDestroyed()
        { // Called from "Damage_Control_Center_CS".
            if (_reloadRoutine != null)
                StopCoroutine(_reloadRoutine);
            inputScript.DisableInput();
            _isDestroyTank = true;
        }


        void Pause(bool isPaused)
        { // Called from "Game_Controller_CS".
            this.enabled = !isPaused;
        }

    }

}