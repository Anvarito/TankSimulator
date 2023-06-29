using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Audio
{
    [CreateAssetMenu(menuName = "Static Data/🎼Music Static Data", fileName = "MusicData")]
    public class MusicStaticData : ScriptableObject
    {
        public List<MusicConfig> Configs;
    }
}