using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresenterBase : MonoBehaviour
{
    protected Canvas _canvas;

    public virtual void InitialCanvas(Canvas canvas, Camera camera)
    {
        _canvas = canvas;
        _canvas.planeDistance = 1;
        SetCamera(camera);
    }

    public virtual void DestroyCanvas()
    {
        Destroy(gameObject);
    }

    public virtual void SetCamera(Camera camera)
    {
        _canvas.worldCamera = camera;
    }
}
