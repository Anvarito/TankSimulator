using UnityEngine;
using UnityEngine.UI;

public class ReloadingCirclePresenter : UIPresenterBase
{
    [SerializeField] private Image _shellImage;
    [SerializeField] private Image _circleImage;

    public void SetLinks(Image shellImage, Image circleImage)
    {
        _shellImage = shellImage;
        _circleImage = circleImage;

        EnableImage(false);
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
