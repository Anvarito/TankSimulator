using UnityEngine;


namespace ChobiAssets.PTM
{

    [DefaultExecutionOrder(+2)] // (Note.) This script is executed after the "Aiming_Control_CS", in order to move the marker smoothly.
    public class AimMarkerUIReceiver : UIRecivierBase
    {
        /*
		 * This script is attached to the "MainBody" of the tank.
		 * This script controls the 'Aim_Marker' in the scene.
         * This script works in combination with "Aiming_Control_CS" in the tank.
		*/

        private AimMarkerPresenter _aimPresenter;
        private Aiming_Control_CS _aimingControl;

        public void Init(Aiming_Control_CS aimingControl, Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup)
        { 
            _aimingControl = aimingControl;
            _gunCamera = gunCamera;
            _cameraSetup = cameraSetup;

            InitialUIRecivier();
        }

        protected override void InstantiateCanvas()
        {
            base.InstantiateCanvas();

            _aimPresenter = _spawnedPresenter as AimMarkerPresenter;
            _aimPresenter.ChangeVisibleMarker(true);
        }

        void Update()
        {
            if (_aimPresenter != null)
            {
                _aimPresenter.ChangeColorMarker(_aimingControl.Target_Rigidbody);
            }
        }

        protected override void SwitchCamera(EActiveCameraType activeCamera)
        {
            base.SwitchCamera(activeCamera);
            _aimPresenter.ChangeVisibleMarker(activeCamera == EActiveCameraType.MainCamera);
        }

        protected override void DestroyUI()
        {
            Destroy(_aimPresenter);
        }
    }
}
