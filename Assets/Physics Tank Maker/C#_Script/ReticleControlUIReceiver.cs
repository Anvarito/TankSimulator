using UnityEngine;

namespace ChobiAssets.PTM
{


	public class ReticleControlUIReceiver : MonoBehaviour
	{
		/*
		 * This script is attached to the "Gun_Camera" under the "Barrel_Base" in the tank.
		 * This script controls the reticle image displayed in the gun camera.
		*/

		// User options >>
		[SerializeField] private Gun_Camera_CS _gunCamera;
		[SerializeField] private ReticleUIPresenter _reticleUIPresenterPrefab;
		[SerializeField] private Camera _camera;
		private ReticleUIPresenter _reticleUIPresenter;
        // << User options

        bool isSelected;

        void Start()
        {
            // Get the reticle image in the scene.
            //if (_gunCamera == null || _reticleUIPresenterPrefab == null)
            //{
            //    Destroy(this);
            //    Debug.LogError("Reticle control is missing links!!!");
            //    return;
            //}

            _reticleUIPresenter = Instantiate(_reticleUIPresenterPrefab);
            _reticleUIPresenter.Initing(_camera);

            _gunCamera.OnEnableGunCam.AddListener(EnableCamera);
            _gunCamera.OnDisableGunCam.AddListener(DisableCamera);
            _gunCamera.OnFOVchange.AddListener(FOVchange);
        }

        public void Initialize(Gun_Camera_CS partsGunCamera)
        {
            _gunCamera = partsGunCamera;
            isSelected = true;
        }

        private void FOVchange(float alpha)
        {
            _reticleUIPresenter.SetRangefinder(alpha);
        }

        private void DisableCamera()
        {
            _reticleUIPresenter.DisableReticle();
            _reticleUIPresenter.SetRangefinder(0);
        }

        private void EnableCamera()
        {
            _reticleUIPresenter.EnableReticle();
        }

        void Selected(bool isSelected)
        { // Called from "ID_Settings_CS".
            if (isSelected)
            {
                this.isSelected = true;
            }
            else
            {
                if (this.isSelected)
                { // This tank is selected until now.
                    this.isSelected = false;
                    //reticleImage.enabled = false;
                }
            }
        }


        void Turret_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS".

            // Turn off the image.
            if (isSelected)
            {
                //reticleImage.enabled = false;
            }

            Destroy(this);
        }


        void Pause(bool isPaused)
        { // Called from "Game_Controller_CS".
            this.enabled = !isPaused;
        }
    }

}

