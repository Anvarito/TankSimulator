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

        public void Load(string name, Action onLoaded = null) =>
            _currentCoroutine = _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string sceneName, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitScene = SceneManager.LoadSceneAsync(sceneName);

            while (!waitScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }

        public void CancelLoading() => 
            _coroutineRunner.StopCoroutine(_currentCoroutine);
    }
}