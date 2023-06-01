using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using System;
using UnityEngine.InputSystem;

public class PlayerInputInitializer : InputInitializerBase
{
    [SerializeField] private Camera_Points_Manager_CS _cameraPointsControl;
    [SerializeField] private Gun_Camera_CS _gunCameraControl;
    [SerializeField] private Camera_Rotation_CS _cameraRotation;


    private Gun_Camera_Input_00_Base_CS _gunCameraControlType;
    private Camera_Points_Manager_Input_00_Base_CS _cameraPoinrsType;
    private Camera_Rotation_Input_00_Base_CS _cameraInput;

    private PlayerInput _playerInput;
    private NewControl _newControl;

    public void SetPlayerInput(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _newControl = new NewControl();
    }

    protected override void TankDestroyed()
    {
        base.TankDestroyed();
        _gunCameraControl.TurretDestroyed();
    }

    protected override void InititalizeControlls()
    {
        base.InititalizeControlls();

        if (_gunCameraControl != null)
            _gunCameraControl.Initialize(_gunCameraControlType);
        else
            Debug.LogError("Gun_Camera_CS is not linked!!!");

        if (_cameraRotation != null)
            _cameraRotation.Initialize(_cameraInput);
        else
            Debug.LogError("Aiming_Control_CS is not linked!!!");

        if (_cameraPointsControl != null)
            _cameraPointsControl.Initialize(_cameraPoinrsType);
        else
            Debug.LogError("Camera_Points_Manager_CS is not linked!!!");
    }


    protected override void SetypControlsByType()
    {
        _driveControlType            = new Drive_Control_Input_03_Single_Stick_CS               (_playerInput, _newControl.TankMovement.Move);
        _fireControlType             = new Cannon_Fire_Input_02_For_Sticks_Drive_CS             (_playerInput, _newControl.TankMovement.Fire, _newControl.TankMovement.Switch);
        _gunCameraControlType        = new GunCameraInput02_ForSingleStickDrive                 (_playerInput, _newControl.TankMovement.Aim, _newControl.TankMovement.ZoomIn, _newControl.TankMovement.ZoomOut);
        _aimingControlType           = new AimingControlStickDrive                              (_playerInput, _newControl.TankMovement.ResetTurret, _newControl.TankMovement.Look);
        _cameraInput                 = new Camera_Rotation_Input_02_For_Single_Stick_Drive_CS   (_playerInput, _newControl.TankMovement.Look, _newControl.TankMovement.ResetTurret);
        _cameraPoinrsType            = new Camera_Points_Manager_Input_02_Gamepad_CS();
    }
}
