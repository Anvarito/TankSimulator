using System.Collections;
using UnityEngine;
using UnityEngine.UI;


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

        private AimMarkerPresenter _aimLeadPresenter;

        protected override void InstantiateCanvas()
        {
            base.InstantiateCanvas();
            // Get the "Aiming_Control_CS" in the tank.
            if (_aimingControl == null)
            {
                Debug.LogWarning("'Aiming_Control_CS' cannot be found in the MainBody.");
            }
            // Get the marker image in the scene.
            if (_uiPrefab == null)
            {
                Debug.LogWarning("'AimMarkerPresenter' cannot be found in the MainBody.");
            }

            _aimLeadPresenter = Instantiate(_uiPrefab) as AimMarkerPresenter;
            _aimLeadPresenter.InitialCanvas(_cameraSetup.GetCamera());
            _aimLeadPresenter.ChangeVisibleMarker(true);
        }

        void Update()
        {
            if (_aimLeadPresenter != null)
            {
                _aimLeadPresenter.ChangeColorMarker(_aimingControl.Target_Rigidbody);
            }
        }

        protected override void SwitchCamera(EActiveCameraType activeCamera)
        {
            base.SwitchCamera(activeCamera);
            _aimLeadPresenter.ChangeVisibleMarker(activeCamera == EActiveCameraType.MainCamera);
        }

        protected override void DestroyUI()
        {
            Destroy(_aimLeadPresenter);
        }
    }
}
