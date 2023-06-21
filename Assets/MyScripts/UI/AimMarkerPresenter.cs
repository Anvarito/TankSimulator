using UnityEngine;
using UnityEngine.UI;

public class AimMarkerPresenter : UIPresenterBase
{
    [SerializeField] private Image _aimMarker;
    [SerializeField] private Color _normalColor;

    public void SetLinks(Image aimMarker)
    {
        _aimMarker = aimMarker;
    }

    public void ChangeVisibleMarker(bool show)
    {
        // Set the appearance.
        _aimMarker.enabled = show;
    }

    public void ChangeColorMarker(bool hasTarget)
    {
        _aimMarker.color = hasTarget ? Color.red : _normalColor;
    }
    /*
    public void AimMarkerControl(Vector3 targetPosition)
    {
        

        // Set the position.
        // Check the player is finding a target using the gun camera now.
        //if (_aimingScript.reticleAimingFlag)
        //{
        //    // Set the marker at the center of the screen.
        //    //_aimMarker.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 128.0f);
        //    return;
        //}
        // Set the marker on the target position.
        Vector3 currentPosition = _camera.WorldToScreenPoint(targetPosition);
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
    */
}
