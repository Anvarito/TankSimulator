using System.Collections;
using System.Collections.Generic;
using ChobiAssets.PTM;
using UnityEngine;
using UnityEngine.UI;

public class LeadMarkerPresenter : UIPresenterBase
{
    [SerializeField] private Image _markerImage;
    private float _calculationTime = 2.0f;

    public void SetLinks(Image markerImage)
    {
        _markerImage = markerImage;
    }

    public void MarkerControl(Vector3 targetPosition, Rigidbody targetRigidbody, Transform buletGeneratorTransform, float bulletVelocity)
    {
        

        // Check the target is locked on now.
        //if (_aimingScript.Target_Transform == null)
        //{ // The target is not locked on.
        //    _markerImage.enabled = false;
        //    return;
        //}
        // Calculate the ballistic.
        var muzzlePos = buletGeneratorTransform.position;
        var targetDir = targetPosition - muzzlePos;
        var targetBase = Vector2.Distance(Vector2.zero, new Vector2(targetDir.x, targetDir.z));
        var velocity = buletGeneratorTransform.forward * bulletVelocity;
        if (targetRigidbody)
        { // The target has a rigidbody.
          // Reduce the target's velocity to help the lead-shooting.
            velocity -= targetRigidbody.velocity;
        }
        var isHit = false;
        var isTank = false;
        var previousPos = muzzlePos;
        var currentPos = previousPos;
        var count = 0.0f;
        while (count < _calculationTime)
        {
            // Get the current position.
            var virtualPos = velocity * count;
            virtualPos.y -= 0.5f * -Physics.gravity.y * Mathf.Pow(count, 2.0f);
            currentPos = virtualPos + muzzlePos;

            // Get the hit point by casting a ray.
            if (Physics.Linecast(previousPos, currentPos, out RaycastHit raycastHit, Layer_Settings_CS.Aiming_Layer_Mask))
            {
                currentPos = raycastHit.point;
                isHit = true;
                if (raycastHit.rigidbody && raycastHit.transform.root.tag != "Finish")
                { // The target has a rigidbody, and it is living.
                    //print(raycastHit.rigidbody.transform.name);
                    isTank = true;
                }
                break;
            }

            // Check the ray has exceeded the target.
            var currenBase = Vector2.Distance(Vector2.zero, new Vector2(virtualPos.x, virtualPos.z));
            if (currenBase > targetBase)
            {
                break;
            }

            previousPos = currentPos;
            count += Time.fixedDeltaTime;
        }

        // Convert the hit point to the screen point.
        var screenPos = _canvas.worldCamera.WorldToScreenPoint(currentPos);
        if (screenPos.z < 0.0f)
        { // The hit point is behind the camera.
            _markerImage.enabled = false;
            return;
        }

        // Set the position.
        _markerImage.enabled = true;
        screenPos.z = 128.0f;
        _markerImage.transform.position = screenPos;

        // Set the appearance.
        if (isHit)
        { // The bullet will hit something.
            if (isTank)
            { // The hit object has a rigidbody.
                _markerImage.color = Color.red;
               // _markerImage.sprite = _rightSprite;
            }
            else
            { // The hit object has no rigidbody.
                _markerImage.color = Color.white;
                //_markerImage.sprite = _wrongSprite;
            }
        }
        else
        { // The bullet will not hit anything.
          //markerImage.color = Color.gray;
        }
    }

    internal void SwitchMode(int mode)
    {
        switch (mode)
        {
            case 0: // Keep the initial positon.
                _markerImage.enabled = false;
                break;
        }
    }
}
