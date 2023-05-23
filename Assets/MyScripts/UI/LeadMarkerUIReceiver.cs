using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
using System;

namespace ChobiAssets.PTM
{

    [DefaultExecutionOrder(+3)] // (Note.) This script is executed after the main camera is moved, in order to move the marker smoothly.
    public class LeadMarkerUIReceiver : UIRecivierBase
    {
        /*
		 * This script is attached to the "MainBody" of the tank with "Aiming_Control_CS".
		 * This script controls the Lead Marker in the scene.
		*/

        // User options >>
        [SerializeField] private Bullet_Generator_CS _bulletGenerator;
        // << User options
        private LeadMarkerPresenter _leadMarkerPresenter;
        private Camera _currentCamera;

        protected override void Subscribes()
        {
            base.Subscribes();
            _aimingControl.OnSwitchMode.AddListener(AimingModeSwitch);
        }
        protected override void InstantiateCanvas()
        {
            base.InstantiateCanvas();
            if (_aimingControl == null)
            {
                Debug.LogError("'Aiming_Control_CS' cannot be found in the MainBody.");
            }
            if (_bulletGenerator == null)
            {
                Debug.LogError("'Bullet_Generator_CS' cannot be found. The cannon cannot get the bullet velocity.");
            }

            _leadMarkerPresenter = Instantiate(_uiPrefab) as LeadMarkerPresenter;
            _leadMarkerPresenter.InitialCanvas(_cameraSetup.GetCamera());
        }
        
        private void AimingModeSwitch()
        {
            _leadMarkerPresenter.SwitchMode(_aimingControl.Mode);
        }

        protected override void SwitchCamera(EActiveCameraType activeCamera)
        {
            base.SwitchCamera(activeCamera);
            _currentCamera = activeCamera == EActiveCameraType.GunCamera ? _cameraSetup.GetGunCamera() : _cameraSetup.GetCamera();
            _leadMarkerPresenter.SetCamera(_currentCamera);
        }

        void LateUpdate()
        {
            if (_leadMarkerPresenter == null || _aimingControl.Mode == 0)
                return;

            _leadMarkerPresenter.MarkerControl(_aimingControl.Target_Position, _aimingControl.Target_Rigidbody, _bulletGenerator.transform, _bulletGenerator.Current_Bullet_Velocity);
        }

        protected override void DestroyUI()
        {
            Destroy(_leadMarkerPresenter.gameObject);
            _aimingControl.OnSwitchMode.RemoveListener(AimingModeSwitch);
        }
    }

}
