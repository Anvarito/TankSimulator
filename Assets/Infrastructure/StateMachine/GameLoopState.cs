using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.KillCounter;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Score;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.Timer;
using System;
using Infrastructure.Services.Input;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.StateMachine
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IInputService _inputService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ITimerService _timer;
        private readonly IKillCounter _killCounter;
        private readonly IScoreCounter _scoreCounter;
        private readonly IProgressService _progress;
        private readonly IStaticDataService _dataService;
        private readonly IPlayerFactory _playerFactory;
        private readonly IEnemyFactory _enemyFactory;

        public GameLoopState(GameStateMachine gameStateMachine, IInputService inputService, ITimerService timer,
            IKillCounter killCounter,
            IScoreCounter scoreCounter, IProgressService progress, IStaticDataService dataService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _timer = timer;
            _killCounter = killCounter;
            _scoreCounter = scoreCounter;
            _progress = progress;
            _dataService = dataService;
            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
        }

        public void Enter()
        {
            _scoreCounter.LoadData();
            TryStartTimer();

            _inputService.OnEscTriggered += PauseGame;
            _enemyFactory.OnEnemyCreate += NewEnemyCreate;
            RegisterKillCounter();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Exit()
        {
            _inputService.OnEscTriggered -= PauseGame;
            _enemyFactory.OnEnemyCreate -= NewEnemyCreate;
            UnregisterKillCounter();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void TryStartTimer()
        {
            GamemodeConfig modeConfig = _dataService.ForMode(_progress.Progress.WorldData.ModeId);
            if (modeConfig.IsGameOverTimerEnabled && !_progress.Progress.WorldData.StartedLevel)
            {
                _timer.StartTimer(modeConfig.GameTime * Constants.SecondInMinute, Victory);
                _progress.Progress.WorldData.StartedLevel = true;
            }

        }

        private void PauseGame() =>
            _gameStateMachine.Enter<PauseState>();

        private void NewEnemyCreate(ID_Settings_CS newEnemy) =>
            _playerFactory.AddNewEnemyToPositionActorsUI(newEnemy);

        private void GameOver() =>
            _gameStateMachine.Enter<DefeatState, List<PlayerData>>(PlayerNamesAndScore());

        private void Victory() =>
            _gameStateMachine.Enter<VictoryState, List<PlayerData>>(PlayerNamesAndScore());

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

        private List<PlayerData> PlayerNamesAndScore()
        {
            List<PlayerData> playerData;

            switch (_progress.Progress.WorldData.ModeId) 
            { 
                case GamemodeId.Survival:
                    playerData = _scoreCounter.Scores.Zip(_inputService.PlayerConfigs, (first, second) => new PlayerData() { Score = first, Config = second }).ToList();
                    // playerData = playerDatas.First(x => x.PlayerNamesAndScore == playerDatas.Max(x => x.PlayerNamesAndScore));
                    break;
                default:
                    return null;
            }

            return playerData;
        }
    }

    public class PlayerData
    {
        public float Score;
        public PlayerConfiguration Config;
    }
}