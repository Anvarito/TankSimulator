using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using System;
using UnityEngine.InputSystem;

public class TankInputInitializer : MonoBehaviour
{
    [Header("Main links")]
    [SerializeField] private ID_Settings_CS _iDSettingsCS;
    [SerializeField] private DamageReciviersManager _damageManager;
    [SerializeField] private Turret_Finishing_CS _turret_Finishing;
    [SerializeField] private CameraViewSetup _cameraViewSetup;

    private PlayerInput _playerInput;
    private NewControl _newControl;

    [Space(30)]
    [SerializeField] private AI_CS ai_core_script;

    [Space(30)]
    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private Drive_Control_CS _driveControl;
    [SerializeField] private Cannon_Fire_CS _fireControl;
    [SerializeField] private Aiming_Control_CS _aimingControl;
    [SerializeField] private Camera_Points_Manager_CS _cameraPointsControl;
    [SerializeField] private Gun_Camera_CS _gunCameraControl;
    [SerializeField] private Camera_Rotation_CS _cameraRotation;


    private Drive_Control_Input_00_Base_CS _driveControlType;
    private Cannon_Fire_Input_00_Base_CS _fireControlType;
    private Aiming_Control_Input_00_Base_CS _aimingControlType;
    private Gun_Camera_Input_00_Base_CS _gunCameraControlType;
    private Camera_Points_Manager_Input_00_Base_CS _cameraPoinrsType;
    private Camera_Rotation_Input_00_Base_CS _cameraInput;

    public void InitializePlayer(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _newControl = new NewControl();

        _turret_Finishing.Launch();

        SetypControlsByType();

        _damageManager.OnTankDestroyed.AddListener(TankDestroyed);
        InititalizeControlls();
    }
    private void TankDestroyed()
    {
        _driveControl.TankDestroyed();
        _fireControl.TankDestroyed();
        _aimingControl.TankDestroyed();

        if (_iDSettingsCS.PlayerType == EPlayerType.Player)
        {
            _gunCameraControl.TurretDestroyed();
        }
        else
        {
            ai_core_script.TankDestroyed();
        }
    }

    private void InititalizeControlls()
    {
        if (_driveControl != null)
            _driveControl.Initialize(_driveControlType);
        else
            Debug.LogError("Drive_Control_CS is not linked!!!");

        if (_fireControl != null)
            _fireControl.Initialize(_fireControlType);
        else
            Debug.LogError("Canon_Fire_CS is not linked!!!");

        if (_aimingControl != null)
            _aimingControl.Initialize(_aimingControlType, _cameraViewSetup.GetCamera());
        else
            Debug.LogError("Aiming_Control_CS is not linked!!!");


        if (_iDSettingsCS.PlayerType == EPlayerType.Player)
        {

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
        else if (_iDSettingsCS.PlayerType == EPlayerType.AI)
        {
            if (_cameraPointsControl != null) Destroy(_cameraPointsControl.gameObject);
            if (_gunCameraControl != null) Destroy(_gunCameraControl.gameObject);
        }
    }


    private void SetypControlsByType()
    {
        switch (_iDSettingsCS.PlayerType)
        {

            case EPlayerType.Player:
                _driveControlType        = new Drive_Control_Input_03_Single_Stick_CS(_playerInput, _newControl.TankMovement.Move);
                _fireControlType         = new Cannon_Fire_Input_02_For_Sticks_Drive_CS(_playerInput, _newControl.TankMovement.Fire, _newControl.TankMovement.Switch);
                _gunCameraControlType    = new GunCameraInput02_ForSingleStickDrive(_playerInput, _newControl.TankMovement.Aim, _newControl.TankMovement.ZoomIn, _newControl.TankMovement.ZoomOut);
                _aimingControlType       = new Aiming_Control_Input_02_For_Single_Stick_Drive_CS(_playerInput, _newControl.TankMovement.ResetTurret, _newControl.TankMovement.Look, _cameraViewSetup.GetCamera());
                _cameraInput             = new Camera_Rotation_Input_02_For_Single_Stick_Drive_CS(_playerInput, _newControl.TankMovement.Look);
                _cameraPoinrsType        = new Camera_Points_Manager_Input_02_Gamepad_CS();
                break;

            case EPlayerType.AI:
                _driveControlType        = new Drive_Control_Input_99_AI_CS();
                _fireControlType         = new Cannon_Fire_Input_99_AI_CS(ai_core_script);
                //_aimingControlType = new Aiming_Control_Input_00_Base_CS();
                break;
        }
    }
}
