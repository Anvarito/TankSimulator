using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewSetup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera _camera;
    [SerializeField] private Camera _gunCamera;

    private Vector2 _aimPosition; 
    private Vector2 _aimReticlePosition; 
    public void SetupLayoutScreen(int playerIndex, int maxPlayer)
    {
        Vector2 position = new Vector2(0, 0.5f * playerIndex);
        Vector2 scale = new Vector2(1, 1.0f / maxPlayer);
        _camera.rect = new Rect(position, scale);
        _gunCamera.rect = new Rect(position, scale);
    }

    public Camera GetCamera()
    {
        return _camera;
    }
    public Camera GetGunCamera()
    {
        return _gunCamera;
    }
    public void SetScreenAimPoint(int playerIndex, int maxPlayers)
    {
        _aimPosition.x = _camera.pixelRect.width * 0.5f;

        float workArea = Screen.height / maxPlayers;
        float heightKoeff = workArea * 0.75f;
        float positionHeight = workArea * playerIndex + heightKoeff;

        _aimPosition.y = positionHeight;

        heightKoeff = workArea * 0.5f;
        positionHeight = workArea * playerIndex + heightKoeff;

        _aimReticlePosition.x = _gunCamera.pixelRect.width * 0.5f;
        _aimReticlePosition.y = positionHeight;
    }
    public Vector2 GetAimPosition()
    {
        return _aimPosition;
    }
    public Vector2 GetReticleAimPosition()
    {
        return _aimReticlePosition;
    }
}
