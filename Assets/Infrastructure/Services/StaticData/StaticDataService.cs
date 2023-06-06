using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.StaticData.Level;
using Infrastructure.Services.StaticData.Tank;
using UnityEngine;

namespace Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelDataPath = "StaticData/Levels";
        private const string TankDataPath = "StaticData/TanksData";

        private Dictionary<TankId, TankConfig> _tanks;
        private Dictionary<LevelId, LevelConfig> _levels;


        public void LoadAllStaticData()
        {
            _tanks = Resources
                .Load<TanksStaticData>(TankDataPath)
                .Tanks
                .ToDictionary(x => x.TankId, x => x);

            _levels = Resources
                .LoadAll<LevelStaticData>(LevelDataPath)
                .Select(x => x.Config)
                .ToDictionary(x => x.LevelId, x => x);
            
            Debug.Log("Static data loaded");
        }

        public TankConfig ForTank(TankId id) =>
            _tanks.TryGetValue(id, out TankConfig config)
                ? config
                : null;

        public LevelConfig ForLevel(LevelId id) =>
            _levels.TryGetValue(id, out LevelConfig config)
                ? config
                : null;
    }
}