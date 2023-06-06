using System;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Tank
{
    [Serializable]
    public class TankConfig
    {
        public TankId TankId;
        public GameObject Prefab;
    }
}