using UnityEngine;
using Infrastructure.Factory.Compose;
using Infrastructure.Factory.Base;
using System;
using Infrastructure.Services.Progress;

namespace Infrastructure.StateMachine
{
    public class GameOverState : IPayloadedState<float>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _playerFactory;
        private readonly IProgressService _progressService;
        public GameOverState(GameStateMachine gameStateMachine, IFactories factories, IProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _playerFactory = factories.Single<IPlayerFactory>();

        }
        public void Enter(float score)
        {
            _playerFactory.GameBoard.ShowDefeatPanel(score);
            _playerFactory.GameBoard.OnExitMenu += Menu;
            _playerFactory.GameBoard.OnRestart += Restart;
        }

        private void Restart() =>
           _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.Level);

        private void Menu() =>
            _gameStateMachine.Enter<ResetState>();

        public void Exit()
        {
            _playerFactory.GameBoard.OnExitMenu -= Menu;
            _playerFactory.GameBoard.OnRestart -= Restart;
        }
    }
}