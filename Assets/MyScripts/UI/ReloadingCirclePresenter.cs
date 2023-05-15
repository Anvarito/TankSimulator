using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadingCirclePresenter : MonoBehaviour
{
    [SerializeField] private Image _shellImage;
    [SerializeField] private Image _circleImage;

    public void Initialized(Camera camera)
    {
        GetComponent<Canvas>().worldCamera = camera;
        GetComponent<Canvas>().planeDistance = 1;
    }

    public void EnableImage(bool value)
    {
        _shellImage.enabled = value;
        _circleImage.enabled = value;
    }

    public void FillCircle(float currentTime, float duration)
    {
        _circleImage.fillAmount = currentTime / duration;
    }
}
