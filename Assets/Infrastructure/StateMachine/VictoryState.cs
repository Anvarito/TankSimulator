using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using Infrastructure.TestMono;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class VictoryState : IPayloadedState<float>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _playerFactory;
        private readonly IInputService _inputService;

        private Dictionary<string, float> _scoreList;

        public VictoryState(GameStateMachine gameStateMachine,IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _playerFactory = factories.Single<IPlayerFactory>();
        }
        
        public void Enter(float score)
        {
            Debug.Log($"Entered {this.GetType().Name}");

            _scoreList = new Dictionary<string, float>()
            {
                ["Van Darkholm"] = 1000,
                ["���������"] = 300,
                ["��������"] = 1500,
                ["����"] = 500,
                ["��������"] = 100,
                ["��������"] = 200,
                ["��������"] = 500,
                ["SUCHKA"] = 800,
                ["�������102"] = 300,
                ["�������"] = 1100,
            };


            ScoreHolder playerScore = new ScoreHolder("Player " + Random.Range(0,99), score);
            _scoreList.Add(playerScore.Name, playerScore.Points);

            _playerFactory.GameBoard.ShowVictoryPanel(_scoreList, playerScore);
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