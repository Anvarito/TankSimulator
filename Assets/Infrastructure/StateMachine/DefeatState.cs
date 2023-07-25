using System;
using System.Collections;
using Infrastructure.Factory.Compose;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Input;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data;
using Infrastructure.Services.Audio;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.Timer;
using Infrastructure.TestMono;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class DefeatState : IPayloadedState<List<PlayerData>>
    {
        private const string ReloadScene = "ReloadScene";

        private Dictionary<string, float> _scoreList;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ITimerService _timerService;
        private readonly IPlayerFactory _playerFactory;
        private readonly IProgressService _progress;
        private readonly IInputService _inputService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAudioService _audioService;
        private readonly IEnemyFactory _enemyFactory;

        public DefeatState(GameStateMachine gameStateMachine, ICoroutineRunner coroutineRunner, IFactories factories,
            ITimerService timerService, IProgressService progress,
            IInputService inputService, ISaveLoadService saveLoadService, IAudioService audioService)
        {
            _gameStateMachine = gameStateMachine;
            _coroutineRunner = coroutineRunner;
            _timerService = timerService;
            _progress = progress;
            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
            _inputService = inputService;
            _saveLoadService = saveLoadService;
            _audioService = audioService;
        }

        public void Enter(List<PlayerData> playerData) =>
            _coroutineRunner.StartCoroutine(WithDelay(playerData));

        public void Exit()
        {
            _playerFactory.GameBoard.OnExitMenu -= Menu;
            _playerFactory.GameBoard.OnRestart -= Restart;
            _enemyFactory.Controller.Pause();
        }

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
                _playerFactory.GameBoard.ShowPanelWithLeaders(leadersList, playerScore, false);
            }
            else
            {
                _playerFactory.GameBoard.ShowEmptyPanel(_playerFactory.PlayersSettings);
            }

            _saveLoadService.SaveProgress();
            _playerFactory.GameBoard.OnExitMenu += Menu;
            _playerFactory.GameBoard.OnRestart += Restart;
        }

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

        private bool IsNotSurvival() =>
            _progress.Progress.WorldData.ModeId != GamemodeId.Survival;

        private void Restart() =>
            _gameStateMachine.Enter<ReloadState, string>(ReloadScene);

        private void Menu() =>
            _gameStateMachine.Enter<ResetState>();
    }
}