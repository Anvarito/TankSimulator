using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Gamemodes
{
    [CreateAssetMenu(menuName = "Static Data/Gamemode Static Data", fileName = "ModsData")]
    public class GamemodeStaticData : ScriptableObject
    {
        public List<GamemodeConfig> Config;
    }
}