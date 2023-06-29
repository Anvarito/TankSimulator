using Infrastructure.Assets;
using Infrastructure.Factory.Base;

namespace Infrastructure.Factory
{
    public class MusicFactory : GameFactory, IMusicFactory
    {
        public MusicFactory(IAssetLoader assetLoader) : base(assetLoader)
        {
        }

        public void CreateMusicSource()
        {
            throw new System.NotImplementedException();
        }

        public void CreateSoundSource()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IMusicFactory
    {
        void CreateMusicSource();
        void CreateSoundSource();
    }
}