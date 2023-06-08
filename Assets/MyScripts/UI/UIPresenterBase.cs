using UnityEngine;

public class UIPresenterBase : MonoBehaviour
{
    [SerializeField] protected Canvas _canvas;

    public Canvas GetCanvas()
    {
        return _canvas;
    }
    public virtual void InitialCanvas()
    {
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
