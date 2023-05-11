using System.Collections.Generic;
using Infrastructure.Assets;
using Infrastructure.Services.Progress;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetLoader _assetLoader;
        private IGameFactory _gameFactoryImplementation;

        public List<IProgressReader> ProgressReaders { get; } = new List<IProgressReader>();
        public List<IProgressWriter> ProgressWriters { get; } = new List<IProgressWriter>();

        public GameFactory(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public GameObject CreatePlayer(GameObject at) =>
            InstantiateRegistered(AssetPaths.PlayerTank, at.transform.position);
        
        public void CreateTankController() =>
            InstantiateRegistered(AssetPaths.TankController);

        public void CreateHud() =>
            InstantiateRegistered(AssetPaths.Hud);

        public void CreateEnemies(GameObject[] at)
        {
            foreach (GameObject point in at)
                _assetLoader.Instantiate(AssetPaths.EnemyTank, point.transform.position);
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private GameObject InstantiateRegistered(string path, Vector3 position)
        {
            GameObject gameObject = _assetLoader.Instantiate(path, at: position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string path)
        {
            GameObject gameObject = _assetLoader.Instantiate(path);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject player)
        {
            foreach (IProgressReader progressReader in player.GetComponentsInChildren<IProgressReader>())
                Register(progressReader);
        }

        private void Register(IProgressReader progressReader)
        {
            if (progressReader is IProgressWriter progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
    }
}