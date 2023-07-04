using System;
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

namespace Infrastructure.StateMachine
{
    public class DefeatState : IPayloadedState<float>
    {
        private const string ReloadScene = "ReloadScene";
        
        private Dictionary<string, float> _scoreList;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ITimerService _timerService;
        private readonly IPlayerFactory _playerFactory;
        private readonly IProgressService _progress;
        private readonly IInputService _inputService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAudioService _audioService;

        public DefeatState(GameStateMachine gameStateMachine, IFactories factories, ITimerService timerService, IProgressService progress,
            IInputService inputService, ISaveLoadService saveLoadService, IAudioService audioService)
        {
            _gameStateMachine = gameStateMachine;
            _timerService = timerService;
            _progress = progress;
            _playerFactory = factories.Single<IPlayerFactory>();
            _inputService = inputService;
            _saveLoadService = saveLoadService;
            _audioService = audioService;
        }

        public void Enter(float score)
        {
            _audioService.PlaySound(SoundId.SadTrombone);
            _timerService.StopTimer();

            _inputService.ResetPlayerIndex();
            _inputService.ConnectToInputs(_playerFactory.GameBoard.transform.root.gameObject, true);

            ScoreHolder playerScore = new ScoreHolder("Player " + UnityEngine.Random.Range(0, 99), score);
            LeadersHolder leadersList = new LeadersHolder();
            leadersList = SetupLeadersHolder(playerScore, leadersList);
            
            _saveLoadService.SaveProgress();
            

            _playerFactory.GameBoard.ShowDefeatPanel(_playerFactory.PlayersSettings, leadersList, playerScore, IsVersus());
            _playerFactory.GameBoard.OnExitMenu += Menu;
            _playerFactory.GameBoard.OnRestart += Restart;
        }

        public void Exit()
        {
            _playerFactory.GameBoard.OnExitMenu -= Menu;
            _playerFactory.GameBoard.OnRestart -= Restart;
        }

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

        private bool IsVersus() => 
            _progress.Progress.WorldData.ModeId == GamemodeId.Versus;

        private void Restart() =>
            _gameStateMachine.Enter<ReloadState, string>(ReloadScene);

        private void Menu() =>
            _gameStateMachine.Enter<ResetState>();
    }
}