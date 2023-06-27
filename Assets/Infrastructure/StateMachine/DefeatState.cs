using UnityEngine;
using Infrastructure.Factory.Compose;
using Infrastructure.Factory.Base;
using System;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Input;
using System.Collections.Generic;
using Infrastructure.Services.SaveLoad;
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

        public DefeatState(GameStateMachine gameStateMachine, IFactories factories, IProgressService progress, IInputService inputService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progress = progress;
            _playerFactory = factories.Single<IPlayerFactory>();
            _inputService = inputService;
            _saveLoadService = saveLoadService;
        }
        public void Enter(float score)
        {
            _inputService.ResetPlayerIndex();
            _inputService.ConnectToInputs(_playerFactory.GameBoard.transform.root.gameObject, true);

            // _progress.Progress.Leaders = new Dictionary<string, float>()
            // {
            //     ["Van Darkholm"] = 1000,
            //     ["Хуепутало"] = 300,
            //     ["ГЛИНОМЕС"] = 1500,
            //     ["Вася"] = 500,
            //     ["какшкин102"] = 300,
            //     ["блядища"] = 1100,
            // };

            ScoreHolder playerScore = new ScoreHolder("Player " + UnityEngine.Random.Range(0, 99), score);
            _progress.Progress.Leaders.Add(playerScore);
            
            _saveLoadService.SaveProgress();
            // _saveLoadService.SaveProgress();
            
            _playerFactory.GameBoard.ShowDefeatPanel(_progress.Progress.Leaders, playerScore);
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