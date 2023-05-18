using System.Collections;
using System.Collections.Generic;
using ChobiAssets.PTM;
using UnityEngine;
using UnityEngine.UI;

public class LeadMarkerPresenter : MonoBehaviour
{
    [SerializeField] private Sprite _wrongSprite;
    [SerializeField] private Sprite _rightSprite;
    [SerializeField] private Image _markerImage;
    [SerializeField] private float _calculationTime = 2.0f;
    private Camera _camera;
    private Camera _gunCamera;
    private Aiming_Control_CS _aimingScript;
    private Bullet_Generator_CS _bullet_Generator_Script;
    private Transform _bulletGeneratorTransform;
    public void Initializing(Aiming_Control_CS aimingScript, Bullet_Generator_CS bulletGenerator, Camera camera, Camera gunCam)
    {
        _gunCamera = gunCam;
        _camera = camera;
        _aimingScript = aimingScript;
        _bullet_Generator_Script = bulletGenerator;
        _bulletGeneratorTransform = _bullet_Generator_Script.transform;

        _markerImage.sprite = _rightSprite;
    }
    public void MarkerControl()
    {
        // Check the aiming mode.
        switch (_aimingScript.Mode)
        {
            case 0: // Keep the initial positon.
                _markerImage.enabled = false;
                return;
        }

        // Check the target is locked on now.
        //if (_aimingScript.Target_Transform == null)
        //{ // The target is not locked on.
        //    _markerImage.enabled = false;
        //    return;
        //}

        // Calculate the ballistic.
        var muzzlePos = _bulletGeneratorTransform.position;
        var targetDir = _aimingScript.Target_Position - muzzlePos;
        Debug.DrawRay(muzzlePos, targetDir, Color.red, 100);
        var targetBase = Vector2.Distance(Vector2.zero, new Vector2(targetDir.x, targetDir.z));
        var bulletVelocity = _bulletGeneratorTransform.forward * _bullet_Generator_Script.Current_Bullet_Velocity;
        if (_aimingScript.Target_Rigidbody)
        { // The target has a rigidbody.
          // Reduce the target's velocity to help the lead-shooting.
            bulletVelocity -= _aimingScript.Target_Rigidbody.velocity;
        }
        var isHit = false;
        var isTank = false;
        var previousPos = muzzlePos;
        var currentPos = previousPos;
        var count = 0.0f;
        while (count < _calculationTime)
        {
            // Get the current position.
            var virtualPos = bulletVelocity * count;
            virtualPos.y -= 0.5f * -Physics.gravity.y * Mathf.Pow(count, 2.0f);
            currentPos = virtualPos + muzzlePos;

            // Get the hit point by casting a ray.
            if (Physics.Linecast(previousPos, currentPos, out RaycastHit raycastHit, Layer_Settings_CS.Aiming_Layer_Mask))
            {
                currentPos = raycastHit.point;
                isHit = true;
                if (raycastHit.rigidbody && raycastHit.transform.root.tag != "Finish")
                { // The target has a rigidbody, and it is living.
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
        Camera camera = _aimingScript.CameraMain.enabled ? _camera : _gunCamera;
        var screenPos = camera.WorldToScreenPoint(currentPos);
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
}
