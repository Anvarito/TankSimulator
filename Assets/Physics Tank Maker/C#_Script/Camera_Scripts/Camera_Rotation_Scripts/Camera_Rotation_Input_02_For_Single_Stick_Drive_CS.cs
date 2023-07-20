using UnityEngine;
using UnityEngine.InputSystem;

namespace ChobiAssets.PTM
{
    public class Camera_Rotation_Input_02_For_Single_Stick_Drive_CS : Camera_Rotation_Input_00_Base_CS
    {
        private InputAction _lookAction;
        private PlayerInput _playerInput;
        private InputAction _resetTurretAction;
        private Vector2 _rotateInput;

        public Camera_Rotation_Input_02_For_Single_Stick_Drive_CS(PlayerInput playerInput, InputAction lookAction,
            InputAction resetTurretAction)
        {
            _lookAction = lookAction;
            _playerInput = playerInput;
            _resetTurretAction = resetTurretAction;
            _playerInput.onActionTriggered += Rotate;
            _playerInput.onActionTriggered += ResetTurret;
        }

        private void ResetTurret(InputAction.CallbackContext obj)
        {
            if (_resetTurretAction.name == obj.action.name)
            {
                // Look forward.
                rotationScript.Look_At_Target(rotationScript.BodyTransform.position +
                                              rotationScript.BodyTransform.forward * 64.0f);
            }
        }

        private void Rotate(InputAction.CallbackContext obj)
        {
            if (_lookAction.name == obj.action.name)
            {
                _rotateInput = obj.action.ReadValue<Vector2>();
                _rotateInput = obj.control.device == Mouse.current ? _rotateInput / 55 : _rotateInput;
            }
        }

        public override void Prepare(Camera_Rotation_CS rotationScript)
        {
            base.Prepare(rotationScript);
            this.rotationScript = rotationScript;
        }


        public override void Get_Input()
        {
            // Check the main camera is enabled.
            if (rotationScript.Main_Camera.enabled == false)
            {
                // Do not rotate.
                rotationScript.Horizontal_Input = 0.0f;
                rotationScript.Vertical_Input = 0.0f;
                return;
            }

            // Rotation.
            //multiplier = Mathf.Lerp(0.1f, rotationScript.RotationMultiplier, rotationScript.Main_Camera.fieldOfView / 15.0f); // Change the rotation speed according to the FOV of the main camera.
            multiplier = 35;
            var vertical = _rotateInput.y;
            var horizontal = _rotateInput.x;

            rotationScript.Horizontal_Input = horizontal * multiplier * Time.deltaTime;
            rotationScript.Vertical_Input = vertical * multiplier * 0.5f * Time.deltaTime;
        }
    }
}