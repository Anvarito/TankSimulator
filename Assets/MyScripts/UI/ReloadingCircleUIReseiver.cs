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
        [SerializeField] private Cannon_Fire_CS _cannonFireScript;

        private ReloadingCirclePresenter _reloadingCirclePresenter;

        private bool _isRun;

        protected override void Subscribes()
        {
            base.Subscribes();
            _cannonFireScript.OnReload.AddListener(ReloadLaunch) ;
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
            //_reloadingCirclePresenter = Instantiate(_uiPrefab) as ReloadingCirclePresenter;
            //_reloadingCirclePresenter.InitialCanvas(_cameraSetup.GetCamera());

            //Canvas canvas = new GameObject("ReloadingCircleCanvas").AddComponent<Canvas>();
            //canvas.renderMode = RenderMode.ScreenSpaceCamera;
            //canvas.planeDistance = 1;
            //canvas.worldCamera = _cameraSetup.GetCamera();
            //canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            //canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            //_reloadingCirclePresenter = canvas.gameObject.AddComponent<ReloadingCirclePresenter>();

            //Image reloadCIrcle = new GameObject("ReloadingCircle").AddComponent<Image>();
            //reloadCIrcle.transform.parent = canvas.transform;
            //reloadCIrcle.sprite = _circleSprite;
            //reloadCIrcle.rectTransform.localPosition = new Vector3(50, 50, 0);
            //reloadCIrcle.rectTransform.pivot = Vector2.zero;
            //reloadCIrcle.rectTransform.anchorMin = Vector2.zero;
            //reloadCIrcle.rectTransform.anchorMax = Vector2.zero;
            //reloadCIrcle.rectTransform.localScale = Vector3.one;
            //reloadCIrcle.rectTransform.sizeDelta = new Vector2(96, 96);
            //reloadCIrcle.color = _colorsHolder.secondaryColor;

            //reloadCIrcle.type = Image.Type.Filled;
            //reloadCIrcle.fillOrigin = (int)Image.Origin360.Top;

            //Image shell = new GameObject("Shell").AddComponent<Image>();
            //shell.transform.parent = reloadCIrcle.transform;
            //shell.sprite = _shellSprite;
            //shell.rectTransform.localPosition = new Vector3(50, 50, 0);
            //shell.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            //shell.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            //shell.rectTransform.localScale = Vector3.one;
            //shell.rectTransform.sizeDelta = new Vector2(96, 96);
            //shell.color = _colorsHolder.secondaryColor;

            _reloadingCirclePresenter = Instantiate(_presenterPrefab) as ReloadingCirclePresenter;
            _reloadingCirclePresenter.InitialCanvas();
            _reloadingCirclePresenter.SetCamera(_cameraSetup.GetCamera());
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