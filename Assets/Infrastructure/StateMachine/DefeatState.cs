using UnityEngine;
using Infrastructure.Factory.Compose;
using Infrastructure.Factory.Base;
using System;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Input;
using System.Collections.Generic;
using Infrastructure.TestMono;

namespace Infrastructure.StateMachine
{
    public class DefeatState : IPayloadedState<float>
    {
        private string _reloadScene = "ReloadScene";
        private Dictionary<string, float> _scoreList;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _playerFactory;
        private readonly IProgressService _progressService;
        private readonly IInputService _inputService;
        public DefeatState(GameStateMachine gameStateMachine, IFactories factories, IProgressService progressService, IInputService inputService)
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

            _scoreList = new Dictionary<string, float>()
            {
                ["Van Darkholm"] = 1000,
                ["Хуепутало"] = 300,
                ["ГЛИНОМЕС"] = 1500,
                ["Вася"] = 500,
                ["Залупкин"] = 100,
                ["Петрович"] = 200,
                ["Дагестан"] = 500,
                ["SUCHKA"] = 800,
                ["какшкин102"] = 300,
                ["блядища"] = 1100,
            };


            ScoreHolder playerScore = new ScoreHolder("Player " + UnityEngine.Random.Range(0, 99), score);
            _scoreList.Add(playerScore.Name, playerScore.Points);

            _playerFactory.GameBoard.ShowDefeatPanel(_scoreList, playerScore);
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