using System;

namespace Infrastructure.Services.StaticData.Level
{
    [Serializable]
    public class LevelConfig
    {
        public LevelId LevelId;
        public string SceneName;
        public string InGameName;
    }
}