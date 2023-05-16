using UnityEngine;
using UnityEngine.InputSystem;

namespace ChobiAssets.PTM
{

    public class Camera_Rotation_Input_02_For_Single_Stick_Drive_CS : Camera_Rotation_Input_00_Base_CS
    {
        private InputAction _lookAction;
        private PlayerInput _playerInput;

        public Camera_Rotation_Input_02_For_Single_Stick_Drive_CS(PlayerInput playerInput, InputAction lookAction)
        {
            _lookAction = lookAction;
            _playerInput = playerInput;

            _playerInput.onActionTriggered += Rotate;
        }

        private void Rotate(InputAction.CallbackContext obj)
        {
            if (_lookAction.name == obj.action.name)
            {
                // Check the main camera is enabled.
                if (rotationScript.Main_Camera.enabled == false)
                {
                    // Do not rotate.
                    rotationScript.Horizontal_Input = 0.0f;
                    rotationScript.Vertical_Input = 0.0f;
                    return;
                }

                // Look forward.
                if (Input.GetKeyDown(General_Settings_CS.Camera_Look_Forward_Pad_Button))
                {
                    rotationScript.Look_At_Target(rotationScript.BodyTransform.position + rotationScript.BodyTransform.forward * 64.0f);
                }

                // Rotation.
                multiplier = Mathf.Lerp(0.1f, rotationScript.RotationMultiplier, rotationScript.Main_Camera.fieldOfView / 15.0f); // Change the rotation speed according to the FOV of the main camera.

                var vertical = obj.action.ReadValue<Vector2>().y;
                var horizontal = obj.action.ReadValue<Vector2>().x;

                rotationScript.Horizontal_Input = horizontal * multiplier;
                rotationScript.Vertical_Input = vertical * multiplier * 0.5f;
            }
        }

        public override void Prepare(Camera_Rotation_CS rotationScript)
        {
            base.Prepare(rotationScript);
            this.rotationScript = rotationScript;
        }


        public override void Get_Input()
        {
            //// Check the main camera is enabled.
            //if (rotationScript.Main_Camera.enabled == false)
            //{
            //    // Do not rotate.
            //    rotationScript.Horizontal_Input = 0.0f;
            //    rotationScript.Vertical_Input = 0.0f;
            //    return;
            //}

            //// Look forward.
            //if (Input.GetKeyDown(General_Settings_CS.Camera_Look_Forward_Pad_Button))
            //{
            //    rotationScript.Look_At_Target(rotationScript.BodyTransform.position + rotationScript.BodyTransform.forward * 64.0f);
            //}

            //// Rotation.
            //multiplier = Mathf.Lerp(0.1f, rotationScript.RotationMultiplier, rotationScript.Main_Camera.fieldOfView / 15.0f); // Change the rotation speed according to the FOV of the main camera.

            //var vertical = _lookAction.ReadValue<Vector2>().y;
            //var horizontal = _lookAction.ReadValue<Vector2>().x;

            //rotationScript.Horizontal_Input = horizontal * multiplier;
            //rotationScript.Vertical_Input = vertical * multiplier * 0.5f;
        }

    }

}
