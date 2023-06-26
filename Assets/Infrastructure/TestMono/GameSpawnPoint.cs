using System.Collections;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.SpawnPoints;
using UnityEngine;

namespace Infrastructure.TestMono
{
    public class GameSpawnPoint : MonoBehaviour
    {
        private IEnemyFactory _enemyFactory;
        private IPlayerFactory _playerFactory;
        private GamemodeConfig _modeConfig;
        private SpawnPointConfig _config;
        
        public void Construct(IFactories factories, SpawnPointConfig config, GamemodeConfig gamemodeConfig)
        {
            _modeConfig = gamemodeConfig;
            _config = config;
            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();

            CreateEnemy();
        }

        private IEnumerator SpawnEnemyPeriodically(float cooldownSpawn, float cooldownRange)
        {
            while (true)
            {
                CreateEnemy();
                float cooldown = Random.Range(cooldownSpawn - cooldownRange, cooldownSpawn + cooldownRange);
                yield return new WaitForSeconds(cooldown);
            }
        }

        private void CreateEnemy()
        {
            transform.position = _config.Position;

            if (_config.ActorType == EPlayerType.AI)
                _enemyFactory.CreateEnemy(_config);

            if (_modeConfig.EnemiesSpawnsPeriodically)
                StartCoroutine(SpawnEnemyPeriodically(_modeConfig.EnemiesCooldownSpawn, _modeConfig.CooldownRange));
        }
    }
}
