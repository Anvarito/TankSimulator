using UnityEngine;
using Infrastructure.Factory.Compose;
using Infrastructure.Factory.Base;
using System;

namespace Infrastructure.StateMachine
{
    public class GameOverState : IPayloadedState<float>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _playerFactory;
        public GameOverState(GameStateMachine gameStateMachine, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _playerFactory = factories.Single<IPlayerFactory>();

        }
        public void Enter(float score)
        {
            Debug.Log($"Entered {this.GetType().Name}");

            _playerFactory.GameBoard.ShowDefeatPanel(score);
            _playerFactory.GameBoard.OnPressContinue += Menu;
        }

        private void Menu()
        {

        }

        public void Exit()
        {
            
        }
    }
}