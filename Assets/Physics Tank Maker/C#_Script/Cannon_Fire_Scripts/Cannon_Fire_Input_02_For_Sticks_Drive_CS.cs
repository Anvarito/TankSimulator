using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChobiAssets.PTM
{

	public class Cannon_Fire_Input_02_For_Sticks_Drive_CS : Cannon_Fire_Input_00_Base_CS
	{
        public override void Prepare(Cannon_Fire_CS cannonFireScript)
        {
            base.Prepare(cannonFireScript);
            General_Settings_CS.InputListener.GetControl().Tank.Fire.performed += Fire;
            General_Settings_CS.InputListener.GetControl().Tank.Switch.performed += SwitchShell;
        }

        private void SwitchShell(InputAction.CallbackContext obj)
        {
            cannonFireScript.SwitchBulletType();
        }

        private void Fire(InputAction.CallbackContext obj)
        {
           cannonFireScript.Fire();
        }

        public override void DisableInput()
        {
            base.DisableInput();
            General_Settings_CS.InputListener.GetControl().Tank.Fire.performed -= Fire;
            General_Settings_CS.InputListener.GetControl().Tank.Switch.performed -= SwitchShell;
        }
    }
}
