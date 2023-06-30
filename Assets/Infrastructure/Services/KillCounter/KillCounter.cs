using System;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Gamemodes;

namespace Infrastructure.Services.KillCounter
{
    public class KillCounter : IKillCounter
    {
        private readonly IProgressService _progressService;
        private readonly IStaticDataService _dataService;
        public Action<ID_Settings_CS> OnEnemiesDestroyed { get; set; }
        public Action OnPlayersDestroyed { get; set; }

        public int PlayersDestroyed { get; private set; }
        public int EnemiesDestroyed { get; private set; }

        private readonly IEnemyFactory _enemyFactory;
        private readonly IPlayerFactory _playerFactory;

        public KillCounter(IFactories factories, IProgressService progressService)
        {
            _progressService = progressService;
            
            _enemyFactory = factories.Single<IEnemyFactory>();
            _playerFactory = factories.Single<IPlayerFactory>();
            
            Subscribe();
        }

        private void Subscribe()
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
            if (AllPlayersDestroyed() || IsVersus())
                OnPlayersDestroyed?.Invoke();
        }

        private bool IsVersus() => 
            _progressService.Progress.WorldData.ModeId == GamemodeId.Versus;

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