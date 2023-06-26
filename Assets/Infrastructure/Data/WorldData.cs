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

        public WorldData(string initialLevel)
        {
            PositionOnLevel = new PositionOnLevel(initialLevel, null);
        }

    }
}