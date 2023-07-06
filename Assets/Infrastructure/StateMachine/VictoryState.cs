using System;
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
using Random = UnityEngine.Random;

namespace Infrastructure.StateMachine
{
    public class VictoryState : IPayloadedState<float>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAudioService _audioService;
        private readonly ITimerService _timerService;
        private readonly IProgressService _progress;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPlayerFactory _playerFactory;
        private readonly IInputService _inputService;


        public VictoryState(GameStateMachine gameStateMachine,IAudioService audioService,ITimerService timerService, IInputService inputService,IFactories factories, IProgressService progress, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _audioService = audioService;
            _timerService = timerService;
            _inputService = inputService;
            _progress = progress;
            _saveLoadService = saveLoadService;
            _playerFactory = factories.Single<IPlayerFactory>();
        }
        
        public void Enter(float score)
        {
            _audioService.StopMusic();
            _timerService.StopTimer();
            _progress.Progress.WorldData.StartedLevel = false;

            _inputService.ResetPlayerIndex();
            _inputService.ConnectToInputs(_playerFactory.GameBoard.transform.root.gameObject, true);

            ScoreHolder playerScore = new ScoreHolder("Player " + Random.Range(0,99), score);
            LeadersHolder leaderList = new LeadersHolder();
            leaderList = SetupLeadersHolder(playerScore, leaderList);
            
            _saveLoadService.SaveProgress();

            _playerFactory.GameBoard.ShowVictoryPanel(_playerFactory.PlayersSettings, leaderList, playerScore,_progress.Progress.WorldData.ModeId == GamemodeId.Versus);
            _playerFactory.GameBoard.OnExitMenu += Menu;
        }


        public void Exit() => 
            _playerFactory.GameBoard.OnExitMenu -= Menu;

        private void Menu() => 
            _gameStateMachine.Enter<ResetState>();

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