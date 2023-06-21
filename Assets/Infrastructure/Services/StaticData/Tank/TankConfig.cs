using System;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Tank
{
    [Serializable]
    public class TankConfig
    {
        public TankId TankId;
        public string PrefabPath;
        public GameObject PrefabEmpty;
        public string Name = "Tank";
        public string Description = "Battle Tank";
    }
}