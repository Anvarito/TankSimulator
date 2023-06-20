using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.Level;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.Services.StaticData.Tank;
using Infrastructure.Services.StaticData.Waypoints;
using UnityEngine;

namespace Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        public Dictionary<LevelId, LevelConfig> Levels { get; private set; }

        public Dictionary<GamemodeId, GamemodeConfig> Mods { get; private set; }

        private const string SpawnPointsDataPath = "StaticData/SpawnPoints";

        private const string WaypointsDataPath = "StaticData/Waypoints/WaypointPacksData";

        private const string LevelDataPath = "StaticData/Levels";

        private const string TankDataPath = "StaticData/TanksData";

        private const string ModsDataPath = "StaticData/ModsData";

        private Dictionary<TankId, TankConfig> _tanks;

        private Dictionary<string, List<SpawnPointConfig>> _spawnPoints;
        private Dictionary<WaypointsPackId, WaypointPackConfig> _waypoints;


        public void LoadAllStaticData()
        {
            _tanks = Resources
                .Load<TanksStaticData>(TankDataPath)
                .Tanks
                .ToDictionary(x => x.TankId, x => x);


            Levels = Resources
                .LoadAll<LevelStaticData>(LevelDataPath)
                .Select(x => x.Config)
                .ToDictionary(x => x.LevelId, x => x);

            Mods = Resources
                .Load<GamemodeStaticData>(ModsDataPath)
                .Config
                .ToDictionary(x => x.ModeId, x => x);

            _spawnPoints = Resources
                .LoadAll<SpawnPointPackData>(SpawnPointsDataPath)
                .Select(x => x.PackConfig)
                .ToDictionary(x => HashForTwoId(x.LevelId, x.PackId), x => x.PointsConfigs);

            _waypoints = Resources
                .Load<WaypointPacksData>(WaypointsDataPath)
                .Packs
                .ToDictionary(x => x.PackId, x => x);


            Debug.Log("Static data loaded");
        }

        public TankConfig ForTank(TankId id) =>
            _tanks.TryGetValue(id, out TankConfig config)
                ? config
                : null;

        public LevelConfig ForLevel(LevelId id) =>
            Levels.TryGetValue(id, out LevelConfig config)
                ? config
                : null;

        public GamemodeConfig ForMode(GamemodeId id) =>
            Mods.TryGetValue(id, out GamemodeConfig config)
                ? config
                : null;

        public WaypointPackConfig ForWaypoints(WaypointsPackId id) =>
            _waypoints.TryGetValue(id, out WaypointPackConfig config)
                ? config
                : null;


        public List<SpawnPointConfig> ForLevelAndMode(LevelId id1, GamemodeId id2) =>
            ForSpawnPointConfigs(HashForTwoId(id1, id2));

        private List<SpawnPointConfig> ForSpawnPointConfigs(string hash) =>
            _spawnPoints.TryGetValue(hash, out List<SpawnPointConfig> configs)
                ? configs
                : null;

        private string HashForTwoId(LevelId id1, GamemodeId id2) =>
            Enum.GetName(typeof(LevelId), id1) + Enum.GetName(typeof(GamemodeId), id2);
    }
}