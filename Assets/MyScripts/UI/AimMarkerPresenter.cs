using ChobiAssets.PTM;
using UnityEngine;
using UnityEngine.UI;

public class AimMarkerPresenter : MonoBehaviour
{
    [SerializeField] private Image _aimMarker;
    [SerializeField] private Image _leadMarker;
    private Camera _camera;
    private Aiming_Control_CS _aimingScript;

    public void AimMarkerControl()
    {
        // Set the appearance.
        switch (_aimingScript.Mode)
        {
            case 0: // Keep the initial positon.
                _aimMarker.enabled = false;
                return;

            case 1: // Free aiming.
            case 2: // Locking on.
                _aimMarker.enabled = true;
                if (_aimingScript.Target_Transform)
                {
                    _aimMarker.color = Color.red;
                }
                else
                {
                    _aimMarker.color = Color.white;
                }
                break;
        }

        // Set the position.
        // Check the player is finding a target using the gun camera now.
        if (_aimingScript.reticleAimingFlag)
        {
            // Set the marker at the center of the screen.
            //_aimMarker.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 128.0f);
            return;
        }
        // Set the marker on the target position.
        Vector3 currentPosition = _camera.WorldToScreenPoint(_aimingScript.Target_Position);
        if (currentPosition.z < 0.0f)
        { // Behind of the camera.
            _aimMarker.enabled = false;
        }
        else
        {
            currentPosition.z = 128.0f;
        }
        //Vector3 screenCenter = new Vector3(0,0, currentPosition.z);
        //screenCenter.x = _camera.scaledPixelWidth * 0.5f;
        //screenCenter.y = _camera.scaledPixelHeight * (0.5f + General_Settings_CS.Aiming_Offset);
        //_aimMarker.transform.position = screenCenter;
    }

    internal void Initializing(Aiming_Control_CS aimingScript, Camera camera)
    {
        _camera = camera;
        _aimingScript = aimingScript;
        GetComponent<Canvas>().worldCamera = camera;
        GetComponent<Canvas>().planeDistance = 1;
    }
}
