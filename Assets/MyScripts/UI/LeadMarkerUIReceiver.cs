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
        [SerializeField] private Sprite _leadSprite;
        // << User options
        private LeadMarkerPresenter _leadMarkerPresenter;
        private Camera _currentCamera;

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
            //_leadMarkerPresenter = Instantiate(_uiPrefab) as LeadMarkerPresenter;
            //_leadMarkerPresenter.InitialCanvas(_cameraSetup.GetCamera());

            Canvas canvas = new GameObject("LeadMarkerCanvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            _leadMarkerPresenter = canvas.gameObject.AddComponent<LeadMarkerPresenter>();

            Image leadMarker = new GameObject("LeadMarker").AddComponent<Image>();
            leadMarker.transform.parent = canvas.transform;
            leadMarker.sprite = _leadSprite;
            leadMarker.rectTransform.localPosition = Vector3.zero;
            leadMarker.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            leadMarker.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            leadMarker.rectTransform.localScale = Vector3.one;
            leadMarker.rectTransform.sizeDelta = new Vector2(24, 24);

            _leadMarkerPresenter.InitialCanvas(canvas, _cameraSetup.GetCamera());
            _leadMarkerPresenter.SetLinks(leadMarker);
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
