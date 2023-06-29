using Infrastructure.Components;

namespace Infrastructure.Factory.Base
{
    public interface IAudioFactory : IGameFactory
    {
        void CreateMusicSource();
        void CreateSoundSource();
        MusicSource MusicSource { get; }
        SoundSource SoundsSource { get; }
    }
}