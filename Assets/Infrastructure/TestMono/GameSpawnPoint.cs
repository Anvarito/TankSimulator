using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.StaticData.SpawnPoints;
using UnityEngine;

namespace Infrastructure.TestMono
{
    public class GameSpawnPoint : MonoBehaviour
    {
        private IEnemyFactory _enemyFactory;
        private IPlayerFactory _playerFactory; 
        private SpawnPointConfig _config;


        private void Start()
        {
            transform.position = _config.Position;

            if (_config.ActorType == EPlayerType.AI)
                _enemyFactory.CreateEnemy(_config);
            //
            // if (_config.ActorType == EPlayerType.Player)
            //     _playerFactory.CratePlayer(_config);
        }

        public void Construct(IFactories factories, SpawnPointConfig config)
        {
            _config = config;
            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
        }
    }
}