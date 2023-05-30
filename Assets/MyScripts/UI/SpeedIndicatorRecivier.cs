using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using UnityEngine.UI;

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
        //_speedIndicatorUI = Instantiate(_uiPrefab) as UI_Speed_Indicator_Control_CS;
        //_speedIndicatorUI.InitialCanvas(_cameraSetup.GetCamera());

        //Canvas canvas = new GameObject("SpeedCanvas").AddComponent<Canvas>();
        //canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //canvas.planeDistance = 1;
        //canvas.worldCamera = _cameraSetup.GetCamera();
        //canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        //_speedIndicatorUIprefab = canvas.gameObject.AddComponent<UI_Speed_Indicator_Control_CS>();

        //Text text = new GameObject("SpeedText").AddComponent<Text>();
        //text.font = _font;
        //text.fontSize = 24;
        //text.transform.parent = canvas.transform;
        //text.rectTransform.localPosition = new Vector2(-60, 22);
        //text.rectTransform.pivot = new Vector2(1, 0);
        //text.rectTransform.anchorMin = new Vector2(1, 0);
        //text.rectTransform.anchorMax = new Vector2(1, 0);
        //text.rectTransform.localScale = Vector3.one;
        //text.rectTransform.sizeDelta = new Vector2(200, 32);
        //text.color = _colorsHolder.secondaryColor;

        _speedIndicatorUI = Instantiate(_presenterPrefab) as UI_Speed_Indicator_Control_CS;
        _speedIndicatorUI.InitialCanvas();
        _speedIndicatorUI.SetCamera(_cameraSetup.GetCamera());
    }

    private void Update()
    {
        if (_speedIndicatorUI == null)
            return;

        _speedIndicatorUI.Update_Speed_Text((int)_drive_Control_CS.Current_Velocity);
    }
}
