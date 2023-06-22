using UnityEngine;
using Infrastructure.Factory.Compose;
using Infrastructure.Factory.Base;
using System;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Input;

namespace Infrastructure.StateMachine
{
    public class GameOverState : IPayloadedState<float>
    {
        private string _reloadScene = "ReloadScene";

        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _playerFactory;
        private readonly IProgressService _progressService;
        private readonly IInputService _inputService;
        public GameOverState(GameStateMachine gameStateMachine, IFactories factories, IProgressService progressService, IInputService inputService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _playerFactory = factories.Single<IPlayerFactory>();
            _inputService = inputService;

        }
        public void Enter(float score)
        {
            Debug.Log($"Entered {this.GetType().Name}");
            _inputService.ResetPlayerIndex();
            _inputService.ConnectToInputs(_playerFactory.GameBoard.transform.root.gameObject, true);
            _playerFactory.GameBoard.ShowDefeatPanel(score);
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