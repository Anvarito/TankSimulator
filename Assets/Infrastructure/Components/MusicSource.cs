using Infrastructure.Services.StaticData.Audio;
using UnityEngine;

namespace Infrastructure.Components
{
    public class MusicSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private bool IsPaused;

        private void Awake() =>
            DontDestroyOnLoad(this);

        public void Play(MusicConfig config)
        {
            _source.clip = config.Track;
            _source.Play();
        }

        public void Pause()
        {
            if (IsPaused)
                _source.Play();
            else
                _source.Pause();
            
            IsPaused = !IsPaused;
        }

        public void Stop() =>
            _source.Stop();
    }
}