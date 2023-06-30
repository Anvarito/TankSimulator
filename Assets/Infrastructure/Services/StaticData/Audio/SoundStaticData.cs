using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Audio
{
    [CreateAssetMenu(menuName = "Static Data/🔊Sounds Static Data", fileName = "SoundsData")]
    public class SoundStaticData : ScriptableObject
    {
        public List<SoundConfig> Configs;
    }
}