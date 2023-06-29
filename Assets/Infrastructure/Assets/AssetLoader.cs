using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Assets
{
    public class AssetLoader : IAssetLoader
    {
        public void CleanUp()
        {
        }

        public GameObject Instantiate(string path, Vector3 at)
        {
            GameObject playerPrefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(playerPrefab, at, Quaternion.identity);
        }

        public GameObject Instantiate(string path)
        {
            GameObject hudPrefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(hudPrefab);
        }
    }
}