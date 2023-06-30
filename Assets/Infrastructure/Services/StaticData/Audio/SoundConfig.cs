using System;
using Infrastructure.Services.Audio;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Audio
{
    [Serializable]
    public class SoundConfig
    {
        public SoundId SoundId;
        public AudioClip Track;
    }
}