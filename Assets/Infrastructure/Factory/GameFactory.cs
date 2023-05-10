using Infrastructure.Assets;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetLoader _assetLoader;
        private IGameFactory _gameFactoryImplementation;

        public GameFactory(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public GameObject CreatePlayer(GameObject at) =>
            _assetLoader.Instantiate(AssetPaths.Player, at.transform.position);

        public void CreateHud() =>
            _assetLoader.Instantiate(AssetPaths.Hud);

        public void CreateEnemies(GameObject[] at)
        {
            foreach (GameObject point in at)
                _assetLoader.Instantiate(AssetPaths.Enemy, point.transform.position);
        }
    }
}