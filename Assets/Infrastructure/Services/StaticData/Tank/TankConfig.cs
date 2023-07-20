using System;
using UnityEngine;
using TMPro;
namespace Infrastructure.Services.StaticData.Tank
{
    [Serializable]
    public class TankConfig
    {
        public TankId TankId;
        public string PrefabPath;
        public GameObject PrefabEmpty;
        public string Name = "Tank";
        public TextMeshProUGUI Description;
    }
}