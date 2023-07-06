using System;
using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.Level;

namespace Infrastructure.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public GamemodeId ModeId; 
        public LevelId LevelId; 
        public string Level;
        public List<ERelationship> Teams;
        public float MusicVolume;
        public float SoundsVolume;
        public bool StartedLevel;

        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel, null);
            Teams = new List<ERelationship>();
            MusicVolume = 10;
            SoundsVolume = 10;
        }

    }
}