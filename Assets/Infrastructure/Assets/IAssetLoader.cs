using Infrastructure.Services;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Assets
{
    public interface IAssetLoader : IService
    {
        GameObject Instantiate(string path, Vector3 at);
        GameObject Instantiate(string path);
        GameObject Instantiate(GameObject gameObject);
        GameObject Instantiate(GameObject gameObject, Vector3 at);
        void InstantiateAsync(string path, UnityAction<float> progress = null, UnityAction<GameObject> onComplete = null);
        public ResourceRequest InstantiateAsync(string path);
        TComponent Instantiate<TComponent>(string path) where TComponent : MonoBehaviour;
        TComponent Instantiate<TComponent>(string path, Vector3 at) where TComponent : MonoBehaviour;
    }
}