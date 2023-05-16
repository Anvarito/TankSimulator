using UnityEngine;
using ChobiAssets.PTM;
public class SpeedIndicatorRecivier : MonoBehaviour
{
    [SerializeField] private UI_Speed_Indicator_Control_CS _speedIndicatorUIPrefab;
    [SerializeField] private Drive_Control_CS _drive_Control_CS;
    private UI_Speed_Indicator_Control_CS _speedIndicatorUI;

    private void Start()
    {
        _speedIndicatorUI = Instantiate(_speedIndicatorUIPrefab);
    }

    private void Update()
    {
        _speedIndicatorUI.Update_Speed_Text((int)_drive_Control_CS.Current_Velocity);
    }

    public void Initialize(Drive_Control_CS partsDriveControl)
    {
        _drive_Control_CS = partsDriveControl;
    }
}
