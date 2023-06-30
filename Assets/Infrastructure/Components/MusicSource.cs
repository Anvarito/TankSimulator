using Infrastructure.Services.StaticData.Audio;
using UnityEngine;

namespace Infrastructure.Components
{
    public class MusicSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;

        private void Awake() =>
            DontDestroyOnLoad(this);

        public void Play(MusicConfig config)
        {
            _source.clip = config.Track;
            _source.Play();
        }

        public void Pause() =>
            _source.Pause();

        public void Stop() =>
            _source.Stop();
    }
}