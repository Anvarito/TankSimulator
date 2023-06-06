using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace ChobiAssets.PTM
{


    public class ReticleControlUIReceiver : UIRecivierBase
    {
        /*
		 * This script is attached to the "Gun_Camera" under the "Barrel_Base" in the tank.
		 * This script controls the reticle image displayed in the gun camera.
		*/

        // User options >>
        private ReticleUIPresenter _reticleUIPresenter;
        // << User options

        public void Init( Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup)
        {
            _gunCamera = gunCamera;
            _cameraSetup = cameraSetup;

            InitialUIRecivier();
        }

        protected override void DestroyUI()
        {
        }

        protected override void InstantiateCanvas()
        {
            base.InstantiateCanvas();

            _reticleUIPresenter = _spawnedPresenter as ReticleUIPresenter;
            _reticleUIPresenter.SetCamera(_cameraSetup.GetGunCamera());
        }

        protected override void Subscribes()
        {
            base.Subscribes();
            _gunCamera.OnFOVchange.AddListener(FOVchange);
        }

        protected override void SwitchCamera(EActiveCameraType activeCamera)
        {
            base.SwitchCamera(activeCamera);
            if (activeCamera == EActiveCameraType.MainCamera)
            {
                _reticleUIPresenter.SetRangefinder(0);
            }
        }

        private void FOVchange(float alpha)
        {
            _reticleUIPresenter.SetRangefinder(alpha);
        }
    }
}

