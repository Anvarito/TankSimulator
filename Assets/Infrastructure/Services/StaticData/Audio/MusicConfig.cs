using System;
using Infrastructure.Services.Audio;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Audio
{
    [Serializable]
    public class MusicConfig
    {
        public MusicId MusicId;
        public AudioClip Track;
    }
}