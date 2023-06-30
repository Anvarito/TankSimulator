using System.Collections.Generic;
using Infrastructure.Assets;
using Infrastructure.Components;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Progress;

namespace Infrastructure.Factory
{
    public class AudioFactory : IAudioFactory
    {
        public List<IProgressWriter> ProgressWriters { get; } = new();
        public List<IProgressReader> ProgressReaders { get; } = new();
        public MusicSource MusicSource { get; private set; }
        public SoundSource SoundsSource { get; private set; }

        private readonly IAudioService _audioService;
        private readonly IAssetLoader _assetLoader;

        public AudioFactory(IAssetLoader assetLoader) => 
            _assetLoader = assetLoader;

        public void CreateMusicSource() =>
            MusicSource = _assetLoader.Instantiate<MusicSource>(AssetPaths.MusicSource);

        public void CreateSoundSource() =>
            SoundsSource = _assetLoader.Instantiate<SoundSource>(AssetPaths.SoundSource);


        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
    }
}