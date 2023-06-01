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

        private AimMarkerPresenter _aimPresenter;

        protected override void InstantiateCanvas()
        {
            base.InstantiateCanvas();
            //// Get the "Aiming_Control_CS" in the tank.
            //if (_aimingControl == null)
            //{
            //    Debug.LogWarning("'Aiming_Control_CS' cannot be found in the MainBody.");
            //}

            //Canvas canvas = new GameObject("AimMarkerCanvas").AddComponent<Canvas>();
            //canvas.renderMode = RenderMode.ScreenSpaceCamera;
            //canvas.planeDistance = 1;
            //canvas.worldCamera = _cameraSetup.GetCamera();
            //canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            //canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            //_aimLeadPresenter = canvas.gameObject.AddComponent<AimMarkerPresenter>();

            //Image aimMarker = new GameObject("AimMarker").AddComponent<Image>();
            //aimMarker.transform.parent = canvas.transform;
            //aimMarker.sprite = _aimMarker;
            //aimMarker.rectTransform.localPosition = Vector3.zero;
            //aimMarker.rectTransform.anchorMin = new Vector2(0.5f, 0.75f);
            //aimMarker.rectTransform.anchorMax = new Vector2(0.5f, 0.75f);
            //aimMarker.rectTransform.localScale = Vector3.one;
            //aimMarker.rectTransform.sizeDelta = new Vector2(48, 48);

            _aimPresenter = Instantiate(_presenterPrefab) as AimMarkerPresenter;
            _aimPresenter.InitialCanvas();
            _aimPresenter.SetCamera(_cameraSetup.GetCamera());
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
