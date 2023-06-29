namespace Infrastructure.Services.Music
{
    public interface IMusicService : IService
    {
        void PlayMusic(MusicId id);
        void PlaySound(SoundId id);

        void ChangeMusicVolume(float volume);
        void ChangeSoundVolume(float volume);
        
        void ChangeMusicState(bool nonMuted);
        void ChangeSoundState(bool nonMuted);
    }
}