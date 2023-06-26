using System;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Gamemodes;

namespace Infrastructure.Services.Score
{
    public class ScoreCounter : IScoreCounter
    {
        private readonly IProgressService _progress;
        private readonly IStaticDataService _dataService;
        private readonly IEnemyFactory _enemyFactory;
        
        private GamemodeConfig _modeConfig;
        
        public float Score { get; private set; }

        public ScoreCounter(IFactories factories, IProgressService progress, IStaticDataService dataService)
        {
            _progress = progress;
            _dataService = dataService;
            _enemyFactory = factories.Single<IEnemyFactory>();

            Setup();
        }

        public void LoadData() => 
            _modeConfig = _dataService.ForMode(_progress.Progress.WorldData.ModeId);

        public void CleanUp() => 
            RemoveListenerOnEnemyDestroyed();

        private void Setup() => 
            AddListenerOnEnemyDestroy();

        private void AddListenerOnEnemyDestroy() => 
            _enemyFactory.OnEnemyDestroyed += AddScore;


        private void RemoveListenerOnEnemyDestroyed() => 
            _enemyFactory.OnEnemyDestroyed -= AddScore;

        private void AddScore() => 
            Score += _modeConfig.PointsForKillingEnemy;
    }
}