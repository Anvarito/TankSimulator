using System;
using UnityEditor;

namespace Infrastructure.Services.StaticData.Level
{
    [Serializable]
    public class LevelConfig
    {
        public LevelId LevelId;
        public SceneAsset Scene;
    }
}