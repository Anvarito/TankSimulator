using Infrastructure.Assets;
using Infrastructure.Components;
using Infrastructure.Factory.Base;

namespace Infrastructure.Factory
{
    public class AudioFactory : GameFactory, IAudioFactory
    {
        public MusicSource MusicSource { get; private set; }
        public SoundSource SoundsSource { get; private set; }


        public AudioFactory(IAssetLoader assetLoader) : base(assetLoader)
        {
        }

        public void CreateMusicSource() => 
            MusicSource = _assetLoader.Instantiate<MusicSource>(AssetPaths.MusicSource);

        public void CreateSoundSource() => 
            SoundsSource = _assetLoader.Instantiate<SoundSource>(AssetPaths.SoundSource);


        public override void CleanUp()
        {
            base.CleanUp();
        }
    }
}