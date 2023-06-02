using System.Linq;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Progress;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string PlayerInitialPoint = "PlayerInitialPoint";
        private const string EnemyInitialPoint = "EnemyInitialPoint";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly IProgressService _progressService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;

            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
        }

        public void Enter(string payload)
        {
            Debug.Log($"Entered {this.GetType().Name}");
            
            _playerFactory.CleanUp();
            _enemyFactory.CleanUp();
            
            _sceneLoader.Load(name: payload, OnLoaded);
        }

        private void OnLoaded()
        {
            InitGameLevel();
            // InformProgressReaders();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }

        private void InformProgressReaders()
        {
            foreach (IProgressReader progress in _playerFactory.ProgressReaders) 
                progress.LoadProgress(_progressService.Progress);
            
            foreach (IProgressReader progress in _enemyFactory.ProgressReaders) 
                progress.LoadProgress(_progressService.Progress);
        }

        private void InitGameLevel()
        {
            _enemyFactory.CreateTankController();
            _enemyFactory.CreateEnemies(at: GameObject.FindGameObjectsWithTag(EnemyInitialPoint));

            var playerPoints = GameObject.FindGameObjectsWithTag(PlayerInitialPoint);
            
            _playerFactory.CreatePlayers(at: playerPoints);
            _playerFactory.CreateTankUiSpawners();
            _playerFactory.CreateHud();
        }
    }
}