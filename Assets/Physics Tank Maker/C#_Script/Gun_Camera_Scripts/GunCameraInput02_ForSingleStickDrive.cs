using UnityEngine.InputSystem;

namespace ChobiAssets.PTM
{

    public class GunCameraInput02_ForSingleStickDrive : Gun_Camera_Input_00_Base_CS
    {
        private bool _isDecreaseView;
        private bool _isEncreaseView;

        public override void Prepare(Gun_Camera_CS gunCameraScript)
        {
            base.Prepare(gunCameraScript);
            General_Settings_CS.InputListener.GetControl().Tank.Aim.performed += Aim;
            General_Settings_CS.InputListener.GetControl().Tank.ZoomIn.performed += ZoomInStart;
            General_Settings_CS.InputListener.GetControl().Tank.ZoomIn.canceled += ZommInCancel;
            General_Settings_CS.InputListener.GetControl().Tank.ZoomOut.performed += ZoomOutStart;
            General_Settings_CS.InputListener.GetControl().Tank.ZoomOut.canceled += ZoomOutCancel;
        }

        public override void DissableInput()
        {
            base.DissableInput();
            General_Settings_CS.InputListener.GetControl().Tank.Aim.performed -= Aim;
            General_Settings_CS.InputListener.GetControl().Tank.ZoomIn.performed -= ZoomInStart;
            General_Settings_CS.InputListener.GetControl().Tank.ZoomIn.canceled -= ZommInCancel;
            General_Settings_CS.InputListener.GetControl().Tank.ZoomOut.performed -= ZoomOutStart;
            General_Settings_CS.InputListener.GetControl().Tank.ZoomOut.canceled -= ZoomOutCancel;
        }

        private void ZoomOutCancel(InputAction.CallbackContext obj)
        {
            _isDecreaseView = false;
        }

        private void ZommInCancel(InputAction.CallbackContext obj)
        {
            _isEncreaseView = false;
        }

        private void ZoomOutStart(InputAction.CallbackContext obj)
        {
            _isDecreaseView = true;
        }

        private void ZoomInStart(InputAction.CallbackContext obj)
        {
            _isEncreaseView = true;
        }

        private void Aim(InputAction.CallbackContext obj)
        {
            if (gunCameraScript.Gun_Camera.enabled)
            { // The gun camera is enabled.
                gunCameraScript.Switch_Mode(1); // Off
            }
            else
            { // The gun camera is disabled.
                gunCameraScript.Switch_Mode(2); // On
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
