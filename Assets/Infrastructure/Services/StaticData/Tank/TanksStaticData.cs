using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Tank
{
    [CreateAssetMenu(menuName = "Static Data/Tanks Static Data",fileName = "TanksData")]
    public class TanksStaticData : ScriptableObject
    {
        public List<TankConfig> Tanks;
    }
}