using Infrastructure.Services.StaticData.Audio;
using UnityEngine;

namespace Infrastructure.Components
{
    public class SoundSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        
        private void Awake() => 
            DontDestroyOnLoad(this);

        public void Play(SoundConfig soundConfig)
        {
            _source.clip = soundConfig.Track;
            _source.Play();
        }

        public void Pause() => 
            _source.Pause();

        public void Stop() =>
            _source.Stop();
    }
}