using UnityEngine.InputSystem;

namespace ChobiAssets.PTM
{

    public class Cannon_Fire_Input_02_For_Sticks_Drive_CS : Cannon_Fire_Input_00_Base_CS
    {
        private InputAction _fireAction;
        private InputAction _switchShelActionl;
        private PlayerInput _playerInput;

        private bool _fireBegin = false;
        public Cannon_Fire_Input_02_For_Sticks_Drive_CS(PlayerInput playerInput, InputAction fireAction, InputAction switchShell)
        {
            _fireAction = fireAction;
            _switchShelActionl = switchShell;
            _playerInput = playerInput;

            _playerInput.onActionTriggered += Fire;
            _playerInput.onActionTriggered += SwitchShell;
        }
        public override void Prepare(Cannon_Fire_CS cannonFireScript)
        {
            base.Prepare(cannonFireScript);

        }

        private void SwitchShell(InputAction.CallbackContext obj)
        {
            if (_switchShelActionl.name == obj.action.name)
                cannonFireScript.SwitchBulletType();
        }

        private void Fire(InputAction.CallbackContext obj)
        {
            if (_fireAction.name == obj.action.name)
            {
                _fireBegin = obj.performed;
            }
        }

        public override void Get_Input()
        {
            if (_fireBegin)
                cannonFireScript.Fire();
        }
        public override void DisableInput()
        {
            base.DisableInput();

            _playerInput.onActionTriggered -= Fire;
            _playerInput.onActionTriggered -= SwitchShell;
        }
    }
}
