using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewSetup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera _camera;
    private Vector2 _aimPosition; 
    public void SetupLayoutScreen(Vector2 position, Vector2 size)
    {
        _camera.rect = new Rect(position, size);
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public void SetScreenAimPointByIndex(int playerIndex, int maxPlayers)
    {
        _aimPosition.x = _camera.pixelRect.width * 0.5f;

        float workArea = Screen.height / maxPlayers;
        float heightKoeff = workArea * 0.75f;
        float positionHeight = workArea * playerIndex + heightKoeff;

        _aimPosition.y = positionHeight;
    }

    public Vector2 GetAimPosition()
    {
        return _aimPosition;
    }
}
