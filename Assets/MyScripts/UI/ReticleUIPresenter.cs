using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleUIPresenter : UIPresenterBase
{
    [SerializeField] private RectTransform _RangefinderImage;

    public void SetRangefinder(float alpha)
    {
        float newZ = Mathf.Lerp(0, 180, alpha);
        _RangefinderImage.localRotation = Quaternion.Euler(new Vector3(_RangefinderImage.localRotation.x, _RangefinderImage.localRotation.y, newZ));
    }
}
