using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.StaticData;
using UnityEngine;
using UnityEngine.Audio;

namespace Infrastructure.Services.Audio
{
    public class AudioService : IAudioService
    {
        private const string AudioMixerPath = "/Audio/Master";
        private const string MusicVolume = "MusicVolume";
        private const string SoundVolume = "SoundVolume";
        
        private const int MixerCoef = 80;

        private readonly IAudioFactory _audioFactory;
        private readonly IStaticDataService _dataService;
        private readonly AudioMixer _mixer;

        public AudioService(IFactories factories, IStaticDataService dataService)
        {
            _audioFactory = factories.Single<IAudioFactory>();
            _dataService = dataService;

            _mixer = Resources.Load<AudioMixer>(AudioMixerPath);
        }

        public void PlayMusic(MusicId id) => 
            _audioFactory.MusicSource.Play(_dataService.ForMusic(id));

        public void PlaySound(SoundId id) => 
            _audioFactory.SoundsSource.Play(_dataService.ForSounds(id));

        public void ChangeMusicVolume(float volume) => 
            _mixer.SetFloat(MusicVolume, volume * MixerCoef - MixerCoef);

        public void ChangeSoundVolume(float volume) => 
            _mixer.SetFloat(SoundVolume, volume * MixerCoef - MixerCoef);

        public void StopMusic() => 
            _audioFactory.MusicSource.Stop();

        public void CleanUp()
        {
        }
    }
}