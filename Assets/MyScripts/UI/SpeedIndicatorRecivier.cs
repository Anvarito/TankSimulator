using ChobiAssets.PTM;

public class SpeedIndicatorRecivier : UIRecivierBase
{
    private Drive_Control_CS _drive_Control_CS;
    private UI_Speed_Indicator_Control_CS _speedIndicatorUI;

    public void Init(Drive_Control_CS drive_Control, Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup)
    {
        _drive_Control_CS = drive_Control;
        _gunCamera = gunCamera;
        _cameraSetup = cameraSetup;

        InitialUIRecivier();
    }

    protected override void DestroyUI()
    {
        Destroy(_speedIndicatorUI.gameObject);
    }

    protected override void InstantiateCanvas()
    {
        base.InstantiateCanvas();

        _speedIndicatorUI = _spawnedPresenter as UI_Speed_Indicator_Control_CS;
    }

    private void Update()
    {
        if (_speedIndicatorUI == null)
            return;

        _speedIndicatorUI.Update_Speed_Text((int)_drive_Control_CS.Current_Velocity);
    }
}
