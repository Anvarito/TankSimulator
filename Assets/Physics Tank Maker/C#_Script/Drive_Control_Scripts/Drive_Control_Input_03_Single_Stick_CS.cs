using UnityEngine;
using UnityEngine.InputSystem;

namespace ChobiAssets.PTM
{

    public class Drive_Control_Input_03_Single_Stick_CS : Drive_Control_Input_02_Keyboard_Pressing_CS
    {
        private InputAction _moveAction;
        private PlayerInput _playerInput;
        public Drive_Control_Input_03_Single_Stick_CS(PlayerInput playerInput, InputAction moveAction)
        {
            _moveAction = moveAction;
            _playerInput = playerInput;

            _playerInput.onActionTriggered += Moved;
        }

        private void Moved(InputAction.CallbackContext obj)
        {
            if (_moveAction.name == obj.action.name)
            {
                vertical = obj.action.ReadValue<Vector2>().y;
                vertical = Mathf.Clamp(vertical, -0.5f, 1.0f);
                vertical = Mathf.Ceil(vertical * 2) / 2;
                // Set "horizontal".
                horizontal = obj.action.ReadValue<Vector2>().x;
                horizontal = Mathf.Floor(horizontal * 2) / 2;
                Set_Values();
            }
        }

        //Dont delete
        public override void Drive_Input()
        {
            
        }

        protected override void Brake_Turn()
        {
            if (horizontal < 0.0f)
            { // Left turn.
                controlScript.L_Input_Rate = 0.0f;
                controlScript.R_Input_Rate = vertical;
            }
            else
            { // Right turn.
                controlScript.L_Input_Rate = -vertical;
                controlScript.R_Input_Rate = 0.0f;
            }

            // Set the "Turn_Brake_Rate".
            controlScript.Turn_Brake_Rate = horizontal;
        }

    }

}
