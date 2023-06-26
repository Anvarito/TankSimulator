using System;
using System.Collections;
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
            onProgress?.Invoke(waitScene.progress);
            while (!waitScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }

        public void CancelLoading()
        {
            if (_currentCoroutine != null)
                _coroutineRunner.StopCoroutine(_currentCoroutine);
        }
    }
}