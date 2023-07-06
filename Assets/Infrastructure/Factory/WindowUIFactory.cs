using System.Collections.Generic;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Progress;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class WindowUIFactory : GameFactory, IWindowUIFactory
    {
        public List<IProgressWriter> ProgressWriters { get; } = new();
        public List<IProgressReader> ProgressReaders { get; } = new();

        public PauseMenu PauseMenu { get; private set; }

        public WindowUIFactory(IAudioService audioService, IAssetLoader assetLoader) : base(audioService, assetLoader)
        {
            
        }

        public PauseMenu CreatePauseMenu() => 
            PauseMenu = InstantiateRegistered<PauseMenu>(AssetPaths.PauseMenu);

        public void DestroyPauseMenu() => 
            Object.Destroy(PauseMenu.gameObject);

        public void CleanUp()
        {
        }
    }
}