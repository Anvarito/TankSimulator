using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.Assets
{
    public interface IAssetLoader : IService
    {
        GameObject Instantiate(string path, Vector3 at);
        GameObject Instantiate(string path);
        TComponent Instantiate<TComponent>(string path) where TComponent : MonoBehaviour;
    }
}