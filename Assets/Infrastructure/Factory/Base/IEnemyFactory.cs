using System;
using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Services.StaticData.SpawnPoints;

namespace Infrastructure.Factory.Base
{
    public interface IEnemyFactory : IGameFactory
    {
        List<DamageReceiversManager> EnemyDamageManagers { get; }
        Action<ID_Settings_CS> OnEnemyDestroyed { get; set; }
        Action<ID_Settings_CS> OnEnemyCreate { get; set; }
        int EnemiesCount { get; }
        Game_Controller_CS Controller { get; }
        public void CreateGameController();
        DamageReceiversManager CreateEnemy(SpawnPointConfig config);
    }
}