using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadingCirclePresenter : UIPresenterBase
{
    [SerializeField] private Image _shellImage;
    [SerializeField] private Image _circleImage;

    public void EnableImage(bool value)
    {
        _shellImage.enabled = value;
        _circleImage.enabled = value;
    }

    public void FillCircle(float currentTime, float duration)
    {
        _circleImage.fillAmount = currentTime / duration;
    }

    internal void SetCurrentCamera(Camera currentCamera)
    {
        _canvas.worldCamera = currentCamera;
    }
}
