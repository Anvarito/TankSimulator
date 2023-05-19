using System;
using System.Collections;
using System.Collections.Generic;
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
        _RangefinderImage.localRotation = Quaternion.Euler(new Vector3(_RangefinderImage.localRotation.x, _RangefinderImage.localRotation.y, newZ));
    }

    internal void Initing(Camera camera)
    {
        GetComponent<Canvas>().worldCamera = camera;
        GetComponent<Canvas>().planeDistance = 1;
    }
}
