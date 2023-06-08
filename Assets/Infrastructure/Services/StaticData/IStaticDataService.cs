using System.Collections.Generic;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.Level;
using Infrastructure.Services.StaticData.Tank;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadAllStaticData();
        TankConfig ForTank(TankId id);
        LevelConfig ForLevel(LevelId id);
        GamemodeConfig ForMode(GamemodeId id);
        Dictionary<LevelId, LevelConfig> Levels { get; }
        Dictionary<GamemodeId, GamemodeConfig> Mods { get; }
        Dictionary<TankId, TankConfig> Tanks{ get; }
    }
}