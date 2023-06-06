using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace ChobiAssets.PTM
{

    public class ReloadingCircleUIReseiver : UIRecivierBase
    {
        /*
		 * This script is attached to the "Cannon_Base" in the tank.
		 * This script controls the loading circle displyaed while the bullet is reloaded.
		 * This script works in combination with "Cannon_Fire_CS" in the tank.
		*/

        private Cannon_Fire_CS _cannonFireScript;

        private ReloadingCirclePresenter _reloadingCirclePresenter;

        private bool _isRun;

        public void Init(Cannon_Fire_CS cannonFireScript, Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup)
        {
            _cannonFireScript = cannonFireScript;
            _gunCamera = gunCamera;
            _cameraSetup = cameraSetup;
            InitialUIRecivier();
        }
        protected override void Subscribes()
        {
            base.Subscribes();
            _cannonFireScript.OnReload.AddListener(ReloadLaunch);
            _cannonFireScript.OnEndReload.AddListener(ReloadEnd);
        }

        protected override void SwitchCamera(EActiveCameraType activeCamera)
        {
            base.SwitchCamera(activeCamera);
            Camera currentCamera = activeCamera == EActiveCameraType.GunCamera ? _cameraSetup.GetGunCamera() : _cameraSetup.GetCamera();
            _reloadingCirclePresenter.SetCamera(currentCamera);
        }



        protected override void InstantiateCanvas()
        {
            base.InstantiateCanvas();
            _reloadingCirclePresenter = _spawnedPresenter as ReloadingCirclePresenter;
            _reloadingCirclePresenter.EnableImage(false);
        }
       
        private void ReloadLaunch()
        {
            _reloadingCirclePresenter.EnableImage(true);
            _isRun = true;
        }
        private void ReloadEnd()
        {
            _reloadingCirclePresenter.EnableImage(false);
            _isRun = false;
        }

        void LateUpdate()
        {
            if (_isRun && _reloadingCirclePresenter != null)
            {
                FillCircle();
            }
        }

        void FillCircle()
        {
            _reloadingCirclePresenter.FillCircle(_cannonFireScript.Loading_Count, _cannonFireScript.Reload_Time);
        }

        protected override void DestroyUI()
        {
            Destroy(_reloadingCirclePresenter.gameObject);
        }
    }

}