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
        // << User options
        private Camera _currentCamera;
        private Bullet_Generator_CS _bulletGenerator;
        private Aiming_Control_CS _aimingControl;

        private LeadMarkerPresenter _leadMarkerPresenter;

        public void Init(Aiming_Control_CS aimingControl, Bullet_Generator_CS bulletGenerator, Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup)
        {
            _bulletGenerator = bulletGenerator;
            _aimingControl = aimingControl;
            _gunCamera = gunCamera;
            _cameraSetup = cameraSetup;

            InitialUIRecivier();
        }
        public Transform GetTargetTransform()
        {
            return _leadMarkerPresenter.TargetTransform;
        }
        protected override void Subscribes()
        {
            base.Subscribes();
            _aimingControl.OnSwitchMode.AddListener(AimingModeSwitch);
        }
        protected override void InstantiateCanvas()
        {
            base.InstantiateCanvas();

            _leadMarkerPresenter = _spawnedPresenter as LeadMarkerPresenter;
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
            _aimingControl.OnSwitchMode.RemoveListener(AimingModeSwitch);
        }
    }

}
