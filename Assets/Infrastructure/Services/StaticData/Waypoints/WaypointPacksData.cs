using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.StaticData.Waypoints
{
    [CreateAssetMenu(menuName = "Static Data/Waypoints Static Data",fileName = "WaypointPacksData")]
    public class WaypointPacksData : ScriptableObject
    {
        public List<WaypointPackConfig> Packs;
    }
}