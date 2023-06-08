using System;
using System.Collections.Generic;
using Infrastructure.Services.StaticData.Level;

namespace Infrastructure.Services.StaticData.SpawnPoints
{
    [Serializable]
    public class SpawnPointsPackConfig
    {
        public SpawnPointsPackId PackId;
        public LevelId LevelId;
        public List<SpawnPointConfig> PointsConfigs;
    }
}