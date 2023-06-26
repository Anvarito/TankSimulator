using System;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;

namespace Infrastructure.Services.KillCounter
{
    public class KillCounter : IKillCounter
    {
        public Action<ID_Settings_CS> OnEnemiesDestroyed { get; set; }
        public Action OnPlayersDestroyed { get; set; }

        public int PlayersDestroyed { get; private set; }
        public int EnemiesDestroyed { get; private set; }

        private readonly IEnemyFactory _enemyFactory;
        private readonly IPlayerFactory _playerFactory;

        public KillCounter(IFactories factories)
        {
            _enemyFactory = factories.Single<IEnemyFactory>();
            _playerFactory = factories.Single<IPlayerFactory>();

            Setup();
        }

        private void Setup()
        {
            _enemyFactory.OnEnemyDestroyed += HandleEnemyDestroy;
            _playerFactory.OnPlayerDestroyed += HandlePlayerDestroy;
        }

        private void Unsubscribe()
        {
            _enemyFactory.OnEnemyDestroyed += HandleEnemyDestroy;
            _playerFactory.OnPlayerDestroyed += HandlePlayerDestroy;
        }

        private void HandlePlayerDestroy()
        {
            if (AllPlayersDestroyed())
                OnPlayersDestroyed?.Invoke();
        }

        private void HandleEnemyDestroy(ID_Settings_CS killer)
        {
            if (AllEnemiesDestroyed())
                OnEnemiesDestroyed?.Invoke(killer);
        }

        private bool AllPlayersDestroyed() =>
            ++PlayersDestroyed >= _playerFactory.PlayerCount;

        private bool AllEnemiesDestroyed() =>
            ++EnemiesDestroyed >= _enemyFactory.EnemiesCount;

        public void CleanUp() => 
            Unsubscribe();
    }
}