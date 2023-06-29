using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.Services.StaticData.Gamemodes
{
    [Serializable]
    public class GamemodeConfig
    {
        public GamemodeId ModeId;

        [Range(1,2)]
        public int PlayerCount;
        public string Name;
        
        [FormerlySerializedAs("EnemiesSpawnsPeriodicaly")] public bool EnemiesSpawnsPeriodically;
        public float EnemiesCooldownSpawn;
        public float CooldownRange;

        public bool PlayersSpawnsPeriodicaly;
        public float PlayersCooldownSpawn;

        public bool IsGameOverTimerEnabled;
        public float GameTime;

        public float PointsForKillingEnemy;
        public float PointsForKillingAllies;

    }

}