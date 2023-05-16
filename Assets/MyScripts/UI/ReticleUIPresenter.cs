using UnityEngine;
using UnityEngine.UI;

public class ReticleUIPresenter : MonoBehaviour
{
    [SerializeField] private Image _reticleImage;
    [SerializeField] private RectTransform _RangefinderImage;

    public void EnableReticle()
    {
        _reticleImage.gameObject.SetActive(true);
    }
    public void DisableReticle()
    {
        _reticleImage.gameObject.SetActive(false);
    }

    public void SetRangefinder(float alpha)
    {
        float newZ = Mathf.Lerp(0, 180, alpha);
        _RangefinderImage.rotation = Quaternion.Euler(new Vector3(0,0, newZ));
    }
}
