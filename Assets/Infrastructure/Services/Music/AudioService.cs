using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.StaticData;

namespace Infrastructure.Services.Music
{
    public class AudioService : IAudioService
    {
        private readonly IAudioFactory _audioFactory;
        private readonly IStaticDataService _dataService;

        public AudioService(IFactories factories, IStaticDataService dataService)
        {
            _audioFactory = factories.Single<IAudioFactory>();
            _dataService = dataService;
        }

        public void PlayMusic(MusicId id) => 
            _audioFactory.MusicSource.Play(_dataService.ForMusic(id));

        public void PlaySound(SoundId id)
        {
        }

        public void ChangeMusicVolume(float volume)
        {
        }

        public void ChangeSoundVolume(float volume)
        {
        }

        public void ChangeMusicState(bool nonMuted)
        {
        }

        public void ChangeSoundState(bool nonMuted)
        {
        }

        public void CleanUp()
        {
        }
    }
}