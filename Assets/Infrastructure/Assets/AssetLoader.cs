using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Infrastructure.Assets
{
    public class AssetLoader : IAssetLoader
    {
        private ICoroutineRunner _coroutineRunner;
        private Coroutine _coroutine;
        public AssetLoader(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void CleanUp()
        {
            if (_coroutine != null)
                _coroutineRunner.StopCoroutine(_coroutine);
        }

        public GameObject Instantiate(string path, Vector3 at)
        {
            GameObject playerPrefab = Resources.Load<GameObject>(path);
            return Instantiate(playerPrefab, at);
        }

        public GameObject Instantiate(string path)
        {
            GameObject hudPrefab = Resources.Load<GameObject>(path);
            return Instantiate(hudPrefab);
        }
        public GameObject Instantiate(GameObject gameObject)
        {
            return Object.Instantiate(gameObject);
        }
        public GameObject Instantiate(GameObject gameObject, Vector3 at)
        {
            return Object.Instantiate(gameObject, at, Quaternion.identity);
        }
        public TComponent Instantiate<TComponent>(string path) where TComponent : MonoBehaviour
        {
            TComponent hudPrefab = Resources.Load<TComponent>(path);
            return Object.Instantiate<TComponent>(hudPrefab);
        }
        public TComponent Instantiate<TComponent>(string path, Vector3 at) where TComponent : MonoBehaviour
        {
            TComponent hudPrefab = Resources.Load<TComponent>(path);
            return Object.Instantiate<TComponent>(hudPrefab, at, Quaternion.identity);
        }

        public void InstantiateAsync(string path, UnityAction<float> progress = null, UnityAction<GameObject> onComplete = null)
        {
            _coroutine = _coroutineRunner.StartCoroutine(LoadAsset(path, progress, onComplete));
        }

        public ResourceRequest InstantiateAsync(string path)
        {
            ResourceRequest asset = Resources.LoadAsync(path);
            return asset;
        }

        private IEnumerator LoadAsset(string path, UnityAction<float> progress = null, UnityAction<GameObject> onComplete = null)
        {
            ResourceRequest asset = Resources.LoadAsync(path);

            while (!asset.isDone)
            {
                progress?.Invoke(asset.progress);
                yield return null;
            }

            onComplete?.Invoke(asset.asset as GameObject);
            yield return null;
        }
    }
}