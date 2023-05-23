using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
public class SpeedIndicatorRecivier : UIRecivierBase
{
    [SerializeField] private Drive_Control_CS _drive_Control_CS;
    private UI_Speed_Indicator_Control_CS _speedIndicatorUI;

    protected override void DestroyUI()
    {
        Destroy(_speedIndicatorUI.gameObject);
    }

    protected override void InstantiateCanvas()
    {
        base.InstantiateCanvas();
        _speedIndicatorUI = Instantiate(_uiPrefab) as UI_Speed_Indicator_Control_CS;
        _speedIndicatorUI.InitialCanvas(_cameraSetup.GetCamera());
    }

    private void Update()
    {
        if (_speedIndicatorUI == null)
            return;

        _speedIndicatorUI.Update_Speed_Text((int)_drive_Control_CS.Current_Velocity);
    }
}
