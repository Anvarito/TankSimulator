using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.Level;
using Infrastructure.Services.StaticData.Tank;
using UnityEngine;

namespace Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        public Dictionary<LevelId, LevelConfig> Levels { get; private set; }

        public Dictionary<GamemodeId, GamemodeConfig> Mods { get; private set; }

        private const string LevelDataPath = "StaticData/Levels";

        private const string TankDataPath = "StaticData/TanksData";

        private const string ModsDataPath = "StaticData/ModsData";

        private Dictionary<TankId, TankConfig> _tanks;


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
    }
}