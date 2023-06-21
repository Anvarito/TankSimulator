using System;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Gamemodes
{
    [Serializable]
    public class GamemodeConfig
    {
        public GamemodeId ModeId;

        [Range(1,2)]
        public int PlayerCount;
        public string Name;
    }

}