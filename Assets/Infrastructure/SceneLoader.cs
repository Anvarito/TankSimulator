using System;
using System.Collections;
using Infrastructure.Assets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private Coroutine _currentCoroutine;
        public SceneLoader(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void Load(string name, Action onLoaded = null, Action<float> onProgress = null) =>
            _currentCoroutine = _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded, onProgress));

        private IEnumerator LoadScene(string sceneName, Action onLoaded = null, Action<float> onProgress = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitScene = SceneManager.LoadSceneAsync(sceneName);

            while (!waitScene.isDone)
            {
                onProgress?.Invoke(waitScene.progress);
                yield return null;
            }

            //ResourceRequest additionalAsset = _assetLoader.InstantiateAsync("MapsAssets/" + sceneName + "Assets");

            //if (additionalAsset != null)
            //{
            //    while (!additionalAsset.isDone)
            //    {
            //        onProgress?.Invoke(additionalAsset.progress);
            //        yield return null;
            //    }

            //    _assetLoader.Instantiate(additionalAsset.asset as GameObject);
            //}

            ResourceRequest additionalAsset = Resources.LoadAsync("MapsAssets/" + sceneName + "Assets");

            while (!additionalAsset.isDone)
            {
                onProgress?.Invoke(additionalAsset.progress);
                yield return null;
            }

            if (additionalAsset.asset != null)
                GameObject.Instantiate(additionalAsset.asset as GameObject);

            onLoaded?.Invoke();
        }

        public void CancelLoading()
        {
            if (_currentCoroutine != null)
                _coroutineRunner.StopCoroutine(_currentCoroutine);
        }
    }
}