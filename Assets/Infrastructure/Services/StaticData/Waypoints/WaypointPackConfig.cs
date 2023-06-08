using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.StaticData.WaypointsPack
{
    [Serializable]
    public class WaypointPackConfig
    {
        public WaypointsPackId PackId;
        public List<Vector3> Points;
    }
}