using System.Collections.Generic;
using Infrastructure.Assets;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Progress;
using UnityEngine;

namespace Infrastructure.Factory.Base
{
    public abstract class GameFactory : IGameFactory
    {
        public List<IProgressReader> ProgressReaders { get; } = new List<IProgressReader>();
        public List<IProgressWriter> ProgressWriters { get; } = new List<IProgressWriter>();

        private readonly IAudioService _audioService;
        protected readonly IAssetLoader _assetLoader;

        protected GameFactory(IAudioService audioService, IAssetLoader assetLoader)
        {
            _audioService = audioService;
            _assetLoader = assetLoader;
        }
        
        public virtual void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
        
        protected GameObject InstantiateRegistered(string path, Vector3 position)
        {
            GameObject gameObject = _assetLoader.Instantiate(path, at: position);
            RegisterProgressWatchers(gameObject);
            ConstructAudioEmitters(gameObject);
            return gameObject;
        }

        protected GameObject InstantiateRegistered(string path)
        {
            GameObject gameObject = _assetLoader.Instantiate(path);
            RegisterProgressWatchers(gameObject);
            ConstructAudioEmitters(gameObject);
            return gameObject;
        }
        
        protected TComponent InstantiateRegistered<TComponent>(string path) where TComponent : MonoBehaviour
        {
            TComponent component = _assetLoader.Instantiate<TComponent>(path);
            RegisterProgressWatchers(component.gameObject);
            ConstructAudioEmitters(component.gameObject);
            return component;
        }

        private void ConstructAudioEmitters(GameObject componentGameObject)
        {
            foreach (IAudioEmitter audioEmiter in componentGameObject.GetComponentsInChildren<IAudioEmitter>())
                audioEmiter.Construct(_audioService);
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