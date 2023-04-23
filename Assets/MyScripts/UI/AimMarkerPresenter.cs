using System;
using System.Collections;
using System.Collections.Generic;
using ChobiAssets.PTM;
using UnityEngine;
using UnityEngine.UI;

public class AimMarkerPresenter : MonoBehaviour
{
    [SerializeField] private Image _aimMarker;
    [SerializeField] private Image _leadMarker;
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
            _aimMarker.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 128.0f);
            return;
        }
        // Set the marker on the target position.
        Vector3 currentPosition = Camera.main.WorldToScreenPoint(_aimingScript.Target_Position);
        if (currentPosition.z < 0.0f)
        { // Behind of the camera.
            _aimMarker.enabled = false;
        }
        else
        {
            currentPosition.z = 128.0f;
        }
        _aimMarker.transform.position = currentPosition;
    }

    internal void Initializing(Aiming_Control_CS aimingScript)
    {
        _aimingScript = aimingScript;
    }
}
