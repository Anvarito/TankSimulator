using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Infrastructure.Factory.Base;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData;
using Infrastructure.Factory.Compose;

namespace Infrastructure.StateMachine
{
    public class ResetState : IState
    {
        private GameStateMachine _gameStateMachine;
        IPlayerFactory _playerFactory;
        IEnemyFactory _enemyFactory;
        IInputFactory _inputFactory;
        IInputService _inputService;
        IProgressService _progressService;
        public ResetState(GameStateMachine gameStateMachine, IFactories factories, IInputService inputService, IProgressService progressService)
        {
            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
            _inputFactory = factories.Single<IInputFactory>();

            _inputService = inputService;
            _progressService = progressService;

            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _playerFactory.CleanUp();
            _enemyFactory.CleanUp();
            _inputFactory.CleanUp();

            _inputService.CleanUp();
            _progressService.CleanUp();

            _gameStateMachine.Enter<SetupFirstInputState>();
        }

        public void Exit()
        {
        }
    }
}
