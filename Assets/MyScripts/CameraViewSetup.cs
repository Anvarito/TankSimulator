using UnityEngine;

public class CameraViewSetup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera _camera;
    public void SetupLayoutScreen(Vector2 position, Vector2 size)
    {
        _camera.rect = new Rect(position, size);
    }

    public Camera GetCamera()
    {
        return _camera;
    }
}
