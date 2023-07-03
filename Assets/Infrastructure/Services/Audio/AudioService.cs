using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.StaticData;
using UnityEngine;
using UnityEngine.Audio;

namespace Infrastructure.Services.Audio
{
    public class AudioService : IAudioService
    {
        private const string AudioMixerPath = "Audio/Master";
        private const string MusicVolume = "MusicVolume";
        private const string SoundVolume = "SoundVolume";
        
        private const int MusicMax = -6;
        private const int SoundsMax = 6;
        private const int MixerCoef = -50;
        private const int MinMixerVolume = -80;

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

        public void ChangeMusicVolume(float volume)
        {
            float mixerVolume = (MusicMax - MixerCoef) * (volume / 10 - 1) + MusicMax;
            mixerVolume = mixerVolume <= MixerCoef ? MinMixerVolume : mixerVolume;
            _mixer.SetFloat(MusicVolume, mixerVolume);
        }

        public void ChangeSoundVolume(float volume)
        {
            float mixerVolume = (SoundsMax - MixerCoef) * (volume / 10 - 1) + SoundsMax;
            mixerVolume = mixerVolume <= MixerCoef ? MinMixerVolume : mixerVolume;
            _mixer.SetFloat(SoundVolume, mixerVolume);
        }

        public void StopMusic() => 
            _audioFactory.MusicSource.Stop();

        public void CleanUp()
        {
        }
    }
}