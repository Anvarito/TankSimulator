using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChobiAssets.PTM
{

    public class GunCameraInput02_ForSingleStickDrive : Gun_Camera_Input_00_Base_CS
    {
        private bool _isDecreaseView;
        private bool _isEncreaseView;
        private InputAction _aimAction, _zoomIn, _zoomOut;
        private PlayerInput _playerInput;

        private bool _isMainCamera = true;
        public GunCameraInput02_ForSingleStickDrive(PlayerInput playerInput, InputAction aimAction, InputAction zoomIn, InputAction zoomOut)
        {
            _aimAction = aimAction;
            _zoomIn = zoomIn;
            _zoomOut = zoomOut;
            _playerInput = playerInput;

            playerInput.onActionTriggered += Aim;
            playerInput.onActionTriggered += ZoomInStart;
            playerInput.onActionTriggered += ZommInCancel;
            playerInput.onActionTriggered += ZoomOutStart;
            playerInput.onActionTriggered += ZoomOutCancel;
        }

        public override void DissableInput()
        {
            base.DissableInput();
            _playerInput.onActionTriggered -= Aim;
            _playerInput.onActionTriggered -= ZoomInStart;
            _playerInput.onActionTriggered -= ZommInCancel;
            _playerInput.onActionTriggered -= ZoomOutStart;
            _playerInput.onActionTriggered -= ZoomOutCancel;
        }

        private void ZoomInStart(InputAction.CallbackContext obj)
        {
            if (obj.action.name == _zoomIn.name && obj.performed)
                _isEncreaseView = true;
        }


        private void ZommInCancel(InputAction.CallbackContext obj)
        {
            if (obj.action.name == _zoomIn.name && obj.canceled)
                _isEncreaseView = false;
        }

        private void ZoomOutStart(InputAction.CallbackContext obj)
        {
            if (obj.action.name == _zoomOut.name && obj.performed)
                _isDecreaseView = true;
        }
        private void ZoomOutCancel(InputAction.CallbackContext obj)
        {
            if (obj.action.name == _zoomOut.name && obj.canceled)
                _isDecreaseView = false;
        }

        private void Aim(InputAction.CallbackContext obj)
        {
            if (obj.action.name == _aimAction.name && obj.performed)
            {
                _isMainCamera = !_isMainCamera;

                if (!_isMainCamera)
                    gunCameraScript.Switch_Mode(EActiveCameraType.GunCamera);
                else
                    gunCameraScript.Switch_Mode(EActiveCameraType.MainCamera);
            }
        }

        public override void Get_Input()
        {
            //// Turn on / off.
            //if (General_Settings_CS.Gun_Camera_While_Pressing)
            //{ // Gun camera is enabled only while pressing the key.
            //    if (Input.GetKeyDown(General_Settings_CS.Gun_Camera_Switch_Pad_Button))
            //    {
            //        gunCameraScript.Switch_Mode(2); // On
            //    }
            //    else if (Input.GetKeyUp(General_Settings_CS.Gun_Camera_Switch_Pad_Button))
            //    {
            //        gunCameraScript.Switch_Mode(1); // Off
            //    }
            //}
            //else
            //{ // Gun camera is turned on / off each time the key is pressed.
            //    if (Input.GetKeyDown(General_Settings_CS.Gun_Camera_Switch_Pad_Button))
            //    {
            //        if (gunCameraScript.Gun_Camera.enabled)
            //        { // The gun camera is enabled.
            //            gunCameraScript.Switch_Mode(1); // Off
            //        }
            //        else
            //        { // The gun camera is disabled.
            //            gunCameraScript.Switch_Mode(2); // On
            //        }
            //    }
            //}

            // Zoom.
            if (gunCameraScript.Gun_Camera.enabled)
            { // The gun camera is enabled.
                float inputValue = 0;
                if (_isEncreaseView)
                {
                    inputValue = -1.0f;
                }
                else if (_isDecreaseView)
                {
                    inputValue = 1.0f;
                }

                gunCameraScript.Zoom_Input = inputValue * 0.05f;
            }

        }

    }

}
