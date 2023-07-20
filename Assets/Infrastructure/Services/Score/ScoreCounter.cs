using System;
using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Gamemodes;

namespace Infrastructure.Services.Score
{
    public class ScoreCounter : IScoreCounter
    {
        public float ScorePlayerOne { get; private set; }
        public float ScorePlayerTwo { get; private set; }
        public List<float> Scores => new List<float> { ScorePlayerOne,ScorePlayerTwo};
        public Action<int> OnEnemiesDestroyed { get; set; }

        private readonly Dictionary<ID_Settings_CS, int> _indexById = new Dictionary<ID_Settings_CS, int>();

        private readonly IProgressService _progress;
        private readonly IStaticDataService _dataService;
        private readonly IEnemyFactory _enemyFactory;
        private GamemodeConfig _modeConfig;

        public ScoreCounter(IFactories factories, IProgressService progress, IStaticDataService dataService)
        {
            _progress = progress;
            _dataService = dataService;
            _enemyFactory = factories.Single<IEnemyFactory>();
            
            Setup();
        }

        public void LoadData() =>
            _modeConfig = _dataService.ForMode(_progress.Progress.WorldData.ModeId);

        public void CleanUp()
        {
            ScorePlayerOne = 0;
            ScorePlayerTwo = 0;
            
            // RemoveListenerOnEnemyDestroyed();
        }

        private void Setup() =>
            AddListenerOnEnemyDestroy();

        private void AddListenerOnEnemyDestroy() =>
            _enemyFactory.OnEnemyDestroyed += AddScore;


        private void RemoveListenerOnEnemyDestroyed() =>
            _enemyFactory.OnEnemyDestroyed -= AddScore;

        private void AddScore(ID_Settings_CS killer)
        {
            if (_modeConfig.ModeId == GamemodeId.Training || killer == null || killer.PlayerType != EPlayerType.Player)
                return;

            int indexKiller = GetIndexPlayer(killer);

            if (indexKiller == 0)
                ScorePlayerOne += _modeConfig.PointsForKillingEnemy;
            else if (indexKiller == 1)
                ScorePlayerTwo += _modeConfig.PointsForKillingEnemy;
            else
                return;

            OnEnemiesDestroyed?.Invoke(indexKiller);
        }

        public void AddPlayerIndex(ID_Settings_CS id, int index) => 
            _indexById.Add(id, index);

        public int GetIndexPlayer(ID_Settings_CS player) =>
            _indexById.TryGetValue(player, out int index)
                ? index
                : -1;
    }
}