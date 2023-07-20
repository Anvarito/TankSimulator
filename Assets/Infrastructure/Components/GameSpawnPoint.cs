using System.Collections;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.SpawnPoints;
using UnityEngine;

namespace Infrastructure.Components
{
    public class GameSpawnPoint : MonoBehaviour
    {
        private IEnemyFactory _enemyFactory;
        private GamemodeConfig _modeConfig;
        private SpawnPointConfig _spawnConfig;

        private DamageReceiversManager _currentActor;

        public void Construct(IFactories factories, SpawnPointConfig config, GamemodeConfig gamemodeConfig)
        {
            _modeConfig = gamemodeConfig;
            _spawnConfig = config;
            _enemyFactory = factories.Single<IEnemyFactory>();

            transform.position = _spawnConfig.Position;
            CreateEnemy();
        }

        private void EnemyDestroyed(ID_Settings_CS victim,ID_Settings_CS killer)
        {
            _currentActor.OnTankDestroyed.RemoveListener(EnemyDestroyed);
            _enemyFactory.Controller.Remove_ID(victim);
            _currentActor = null;

            if (_modeConfig.EnemiesSpawnsPeriodically)
                StartCoroutine(SpawnEnemyPeriodically(_modeConfig.EnemiesCooldownSpawn, _modeConfig.CooldownRange));
        }

        private IEnumerator SpawnEnemyPeriodically(float cooldownSpawn, float cooldownRange)
        {
            float cooldown = Random.Range(cooldownSpawn - cooldownRange, cooldownSpawn + cooldownRange);
            yield return new WaitForSeconds(cooldown);
            CreateEnemy();
        }

        private void CreateEnemy()
        {
            if (_spawnConfig.ActorType == EPlayerType.AI)
            {
                _currentActor = _enemyFactory.CreateEnemy(_spawnConfig);
                _currentActor.OnTankDestroyed.AddListener(EnemyDestroyed);
                _enemyFactory.Controller.Receive_ID_Script(_currentActor.GetComponentInParent<ID_Settings_CS>());
            }
        }
    }
}