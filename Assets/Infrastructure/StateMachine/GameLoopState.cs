using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Factory;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.KillCounter;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Score;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.Timer;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameLoopState : IState
    {
        private const float _pointForEnemy = 100f;

        private readonly GameStateMachine _gameStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ITimerService _timer;
        private readonly IKillCounter _killCounter;
        private readonly IScoreCounter _scoreCounter;
        private readonly IProgressService _progress;
        private readonly IStaticDataService _dataService;

        private int _allEnemyDestroyed;
        private int _playerEnemyDestroyed;
        private int _playerDestroyed;
        private Coroutine _gameTimeCoroutine;

        public GameLoopState(GameStateMachine gameStateMachine, ITimerService timer, IKillCounter killCounter, IScoreCounter scoreCounter, IProgressService progress, IStaticDataService dataService)
        {
            _gameStateMachine = gameStateMachine;
            _timer = timer;
            _killCounter = killCounter;
            _scoreCounter = scoreCounter;
            _progress = progress;
            _dataService = dataService;
        }

        public void Enter()
        {
            RegisterKillCounter();

            GamemodeConfig modeConfig = _dataService.ForMode(_progress.Progress.WorldData.ModeId);
            _timer.StartTimer(modeConfig.GameTime * Constants.SecondInMinute, GameOver);
            _scoreCounter.LoadData();
        }

        public void Exit()
        {
            UnregisterKillCounter();

            if (!_timer.IsPaused)
                _timer.PauseTimer();

        }

        private void GameOver() =>
            _gameStateMachine.Enter<DefeatState, float>(_scoreCounter.Score);
        
        private void Victory(ID_Settings_CS killer) =>
            _gameStateMachine.Enter<VictoryState, float>(_scoreCounter.Score);
        

        private void RegisterKillCounter()
        {
            _killCounter.OnEnemiesDestroyed += Victory;
            _killCounter.OnPlayersDestroyed += GameOver;
        }

        private void UnregisterKillCounter()
        {
            _killCounter.OnEnemiesDestroyed -= Victory;
            _killCounter.OnPlayersDestroyed -= GameOver;
        }
    }
}