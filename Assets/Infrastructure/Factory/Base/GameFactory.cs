using System.Collections.Generic;
using Infrastructure.Assets;
using Infrastructure.Services.Progress;
using UnityEngine;

namespace Infrastructure.Factory.Base
{
    public abstract class GameFactory : IGameFactory
    {
        public List<IProgressReader> ProgressReaders { get; } = new List<IProgressReader>();
        public List<IProgressWriter> ProgressWriters { get; } = new List<IProgressWriter>();
        
        protected readonly IAssetLoader _assetLoader;

        protected GameFactory(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
        
        protected GameObject InstantiateRegistered(string path, Vector3 position)
        {
            GameObject gameObject = _assetLoader.Instantiate(path, at: position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        protected GameObject InstantiateRegistered(string path)
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