using System;
using System.Collections;
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
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.StateMachine
{
    public class VictoryState : IPayloadedState<PlayerData>
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


        public VictoryState(GameStateMachine gameStateMachine, ICoroutineRunner coroutineRunner,IAudioService audioService,ITimerService timerService, IInputService inputService,IFactories factories, IProgressService progress, ISaveLoadService saveLoadService)
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
        
        public void Enter(PlayerData playerData) => 
            _coroutineRunner.StartCoroutine(WithDelay(playerData));
        
        public void Exit() => 
            _playerFactory.GameBoard.OnExitMenu -= Menu;

        private void Menu() => 
            _gameStateMachine.Enter<ResetState>();

        private void Restart()
        {
            _enemyFactory.Controller.Pause();
            _gameStateMachine.Enter<ReloadState, string>(ReloadScene);
        }
        
        private IEnumerator WithDelay(PlayerData playerData)
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

            ScoreHolder playerScore = new ScoreHolder(playerData.Config.PlayerName, playerData.Score);
            LeadersHolder leaderList = new LeadersHolder();
            leaderList = SetupLeadersHolder(playerScore, leaderList);
            
            _saveLoadService.SaveProgress();

            _playerFactory.GameBoard.ShowVictoryPanel(_playerFactory.PlayersSettings, leaderList, playerScore, IsNotSurvival());
            _playerFactory.GameBoard.OnExitMenu += Menu;
            _playerFactory.GameBoard.OnRestart += Restart;
        }

        private bool IsNotSurvival() =>
           _progress.Progress.WorldData.ModeId != GamemodeId.Survival;

        private LeadersHolder SetupLeadersHolder(ScoreHolder playerScore, LeadersHolder copyList)
        {
            switch (_progress.Progress.WorldData.ModeId)
            {
                case GamemodeId.Coop:
                    _progress.Progress.LeadersСoop.Add(playerScore);
                    copyList = _progress.Progress.LeadersСoop;
                    break;
                case GamemodeId.Survival:
                    _progress.Progress.LeadersSurvival.Add(playerScore);
                    copyList = _progress.Progress.LeadersSurvival;
                    break;
                case GamemodeId.Versus:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return copyList;
        }
    }
}