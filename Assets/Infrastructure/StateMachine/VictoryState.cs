using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using Infrastructure.TestMono;
using System.Collections.Generic;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticData.Gamemodes;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class VictoryState : IPayloadedState<float>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IProgressService _progress;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPlayerFactory _playerFactory;
        private readonly IInputService _inputService;


        public VictoryState(GameStateMachine gameStateMachine, IInputService inputService,IFactories factories, IProgressService progress, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _progress = progress;
            _saveLoadService = saveLoadService;
            _playerFactory = factories.Single<IPlayerFactory>();
        }
        
        public void Enter(float score)
        {
            Debug.Log($"Entered {this.GetType().Name}");

            _inputService.ResetPlayerIndex();
            _inputService.ConnectToInputs(_playerFactory.GameBoard.transform.root.gameObject, true);

            ScoreHolder playerScore = new ScoreHolder("Player " + Random.Range(0,99), score);
            _progress.Progress.Leaders.Add(playerScore);
            
            _saveLoadService.SaveProgress();

            _playerFactory.GameBoard.ShowVictoryPanel(_playerFactory.PlayersSettings,_progress.Progress.Leaders, playerScore,_progress.Progress.WorldData.ModeId == GamemodeId.Versus);
            _playerFactory.GameBoard.OnExitMenu += Menu;
        }


        private void Menu() => 
            _gameStateMachine.Enter<ResetState>();

        public void Exit()
        {
            _playerFactory.GameBoard.OnExitMenu -= Menu;
        }
    }
}