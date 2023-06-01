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
        protected override void InstantiateCanvas()
        {
            base.InstantiateCanvas();

            //Canvas canvas = new GameObject("ReticleCanvas").AddComponent<Canvas>();
            //canvas.renderMode = RenderMode.ScreenSpaceCamera;
            //canvas.planeDistance = 1;
            //canvas.worldCamera = _cameraSetup.GetGunCamera();
            //canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            //canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            //_reticleUIPresenter = canvas.gameObject.AddComponent<ReticleUIPresenter>();

            //Image reticle = new GameObject("Reticle").AddComponent<Image>();
            //reticle.transform.parent = canvas.transform;
            //reticle.sprite = _reticleSprite;
            //reticle.rectTransform.localPosition = Vector3.zero;
            //reticle.rectTransform.anchorMin = Vector2.zero;
            //reticle.rectTransform.anchorMax = Vector2.one;
            //reticle.rectTransform.localScale = Vector3.one;
            //reticle.rectTransform.sizeDelta = Vector3.zero;
            //reticle.preserveAspect = true;

            //Image rangefinder = new GameObject("Rangefinder").AddComponent<Image>();
            //rangefinder.transform.parent = reticle.transform;
            //rangefinder.sprite = _rengeSprite;
            //rangefinder.rectTransform.localPosition = new Vector3(0, 0, 0);
            //rangefinder.rectTransform.anchorMin = new Vector2(0, 0);
            //rangefinder.rectTransform.anchorMax = new Vector2(1, 1);
            //rangefinder.rectTransform.localScale = new Vector3(0.68f, 0.68f, 0.68f);
            //rangefinder.rectTransform.sizeDelta = new Vector2(0, 0);
            //rangefinder.preserveAspect = true;

            _reticleUIPresenter = Instantiate(_presenterPrefab) as ReticleUIPresenter;
            _reticleUIPresenter.InitialCanvas();
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

        protected override void DestroyUI()
        {
            Destroy(_reticleUIPresenter.gameObject);
        }
    }
}

