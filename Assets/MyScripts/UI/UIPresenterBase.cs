using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresenterBase : MonoBehaviour
{
    [SerializeField] protected Canvas _canvas;

    public virtual void InitialCanvas(Camera camera)
    {
        SetCamera(camera);
        _canvas.planeDistance = 1;
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
