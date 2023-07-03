using Infrastructure.Factory.Compose;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Input;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.Audio;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.TestMono;

namespace Infrastructure.StateMachine
{
    public class DefeatState : IPayloadedState<float>
    {
        private string _reloadScene = "ReloadScene";
        private Dictionary<string, float> _scoreList;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _playerFactory;
        private readonly IProgressService _progress;
        private readonly IInputService _inputService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAudioService _audioService;

        public DefeatState(GameStateMachine gameStateMachine, IFactories factories, IProgressService progress, IInputService inputService, ISaveLoadService saveLoadService, IAudioService audioService)
        {
            _gameStateMachine = gameStateMachine;
            _progress = progress;
            _playerFactory = factories.Single<IPlayerFactory>();
            _inputService = inputService;
            _saveLoadService = saveLoadService;
            _audioService = audioService;
        }
        public void Enter(float score)
        {
            _audioService.PlaySound(SoundId.SadTrombone);
            
            _inputService.ResetPlayerIndex();
            _inputService.ConnectToInputs(_playerFactory.GameBoard.transform.root.gameObject, true);

            ScoreHolder playerScore = new ScoreHolder("Player " + UnityEngine.Random.Range(0, 99), score);
            _progress.Progress.Leaders.Add(playerScore);
            
            _saveLoadService.SaveProgress();
            
            _playerFactory.GameBoard.ShowDefeatPanel(_playerFactory.PlayersSettings,_progress.Progress.Leaders, playerScore, _progress.Progress.WorldData.ModeId == GamemodeId.Versus);
            _playerFactory.GameBoard.OnExitMenu += Menu;
            _playerFactory.GameBoard.OnRestart += Restart;
        }

        private void Restart() =>
           _gameStateMachine.Enter<ReloadState, string>(_reloadScene);

        private void Menu() =>
            _gameStateMachine.Enter<ResetState>();

        public void Exit()
        {
            _playerFactory.GameBoard.OnExitMenu -= Menu;
            _playerFactory.GameBoard.OnRestart -= Restart;
        }
    }
}