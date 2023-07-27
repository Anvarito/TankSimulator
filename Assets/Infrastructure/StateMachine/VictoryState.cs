using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using Infrastructure.TestMono;
using Infrastructure.Data;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.Timer;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.StateMachine
{
    public class VictoryState : IPayloadedState<List<PlayerData>>
    {
        private const string ReloadScene = "ReloadScene";

        private readonly GameStateMachine _gameStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IAudioService _audioService;
        private readonly ITimerService _timerService;
        private readonly IProgressService _progress;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPlayerFactory _playerFactory;
        private readonly IInputService _inputService;
        private readonly IEnemyFactory _enemyFactory;


        public VictoryState(GameStateMachine gameStateMachine, ICoroutineRunner coroutineRunner,
            IAudioService audioService, ITimerService timerService, IInputService inputService, IFactories factories,
            IProgressService progress, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _coroutineRunner = coroutineRunner;
            _audioService = audioService;
            _timerService = timerService;
            _inputService = inputService;
            _progress = progress;
            _saveLoadService = saveLoadService;
            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();

        }

        public void Enter(List<PlayerData> playerData) =>
            _coroutineRunner.StartCoroutine(WithDelay(playerData));

        public void Exit()
        {
            _playerFactory.GameBoard.OnExitMenu -= Menu;
            _enemyFactory.Controller.Pause();
        }

        private void Menu() =>
            _gameStateMachine.Enter<ResetState>();

        private void Restart() =>
            _gameStateMachine.Enter<ReloadState, string>(ReloadScene);


        private IEnumerator WithDelay(List<PlayerData> playerData)
        {
            float endTime = Time.time + Constants.GameOverDelay;
            while (endTime > Time.time)
                yield return null;

            _audioService.StopMusic();
            _enemyFactory.Controller.Pause();
            _timerService.StopTimer();
            _progress.Progress.WorldData.StartedLevel = false;

            _inputService.ResetPlayerIndex();
            _inputService.ConnectToInputs(_playerFactory.GameBoard.transform.root.gameObject, true);

            if (playerData != null)
            {
                var playerScore = playerData.Select(x => new ScoreHolder(x.Config.PlayerName, x.Score)).ToList();
                LeadersHolder leadersList = new LeadersHolder();
                leadersList = SetupLeadersHolder(playerScore, leadersList);
                _playerFactory.GameBoard.ShowPanelWithLeaders(leadersList, playerScore, true);
            }
            else
            {
                _playerFactory.GameBoard.ShowEmptyPanel(_playerFactory.PlayersSettings);
            }

            _saveLoadService.SaveProgress();
            _playerFactory.GameBoard.OnExitMenu += Menu;
            _playerFactory.GameBoard.OnRestart += Restart;
        }

        private bool IsNotSurvival() =>
            _progress.Progress.WorldData.ModeId != GamemodeId.Survival;

        private LeadersHolder SetupLeadersHolder(List<ScoreHolder> playerScore, LeadersHolder copyList)
        {
            switch (_progress.Progress.WorldData.LevelId)
            {
                case Services.StaticData.Level.LevelId.Factory:
                    _progress.Progress.FactoryLeaders.Add(playerScore);
                    copyList = _progress.Progress.FactoryLeaders;
                    break;
                case Services.StaticData.Level.LevelId.Poligon:
                    _progress.Progress.PoligonLeaders.Add(playerScore);
                    copyList = _progress.Progress.PoligonLeaders;
                    break;
                case Services.StaticData.Level.LevelId.Winter:
                    _progress.Progress.WinterLeaders.Add(playerScore);
                    copyList = _progress.Progress.WinterLeaders;
                    break;
                case Services.StaticData.Level.LevelId.Summer:
                    _progress.Progress.SummerLeaders.Add(playerScore);
                    copyList = _progress.Progress.SummerLeaders;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return copyList;
        }
    }
}