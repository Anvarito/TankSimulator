using System.Collections.Generic;
using Infrastructure.Services.Audio;
using Infrastructure.Services.StaticData.Audio;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.Level;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.Services.StaticData.Tank;
using Infrastructure.Services.StaticData.Waypoints;

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
        
        List<SpawnPointConfig> ForLevelAndMode(LevelId id1, GamemodeId id2);
        WaypointPackConfig ForWaypoints(WaypointsPackId configWaypointsPackId);

        Dictionary<TankId, TankConfig> Tanks{ get; }
        MusicConfig ForMusic(MusicId id);
        SoundConfig ForSounds(SoundId id);
    }
}