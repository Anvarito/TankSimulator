using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;

public class InputIniializeManager : MonoBehaviour
{

    [SerializeField] private Drive_Control_CS _driveControl;
    [SerializeField] private Aiming_Control_CS _aimingControl;
    [SerializeField] private Camera_Points_Manager_CS _cameraPointsControl;
    [SerializeField] private Gun_Camera_CS _gunCameraControl;

    private ID_Settings_CS _iDSettingsCS;

    private Drive_Control_Input_00_Base_CS _driveControlType;
    private Aiming_Control_Input_00_Base_CS _aimingControlType;
    private Gun_Camera_Input_00_Base_CS _gunCameraControlType;
    private Camera_Points_Manager_Input_00_Base_CS _cameraInputType;

    private void Awake()
    {
        SetDriveControlVariant();
    }

    private void Start()
    {
        if (_driveControl != null)
            _driveControl.Initialize(_driveControlType);
        else
            Debug.LogError("Drive_Control_CS is not linked!!!");

        if (_aimingControl != null)
            _aimingControl.Initialize(_aimingControlType);
        else
            Debug.LogError("Aiming_Control_CS is not linked!!!");


        if (_iDSettingsCS.PlayerType == EPlayerType.Player)
        {
            if (_cameraPointsControl != null)
                _cameraPointsControl.Initialize(_cameraInputType);
            else
                Debug.LogError("Camera_Points_Manager_CS is not linked!!!");

            if (_gunCameraControl != null)
                _gunCameraControl.Initialize(_gunCameraControlType);
            else
                Debug.LogError("Gun_Camera_CS is not linked!!!");
        }
        else if (_iDSettingsCS.PlayerType == EPlayerType.AI)
        {
            if (_cameraPointsControl != null) Destroy(_cameraPointsControl.gameObject);
            if (_gunCameraControl != null) Destroy(_gunCameraControl.gameObject);
        }
    }

    private void SetDriveControlVariant()
    {
        _iDSettingsCS = GetComponent<ID_Settings_CS>();
        switch (_iDSettingsCS.PlayerType)
        {

            case EPlayerType.Player:
                _driveControlType = new Drive_Control_Input_03_Single_Stick_CS();
                _cameraInputType = new Camera_Points_Manager_Input_02_Gamepad_CS();
                _gunCameraControlType = new GunCameraInput02_ForSingleStickDrive();
                _aimingControlType = new Aiming_Control_Input_02_For_Single_Stick_Drive_CS();
                break;

            case EPlayerType.AI:
                _driveControlType = new Drive_Control_Input_99_AI_CS();
                _cameraInputType = new Camera_Points_Manager_Input_01_Mouse_CS();
                _gunCameraControlType = new Gun_Camera_Input_01_Mouse_CS();
                break;
        }
    }
}
