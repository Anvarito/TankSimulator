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
            TeamSeparator teamSeparator = Object.FindObjectOfType<TeamSeparator>();

            CreateEnemys(teamSeparator);

            CreatePlayers(teamSeparator);
        }

        private void CreatePlayers(TeamSeparator teamSeparator)
        {
            _playerFactory.CreatePlayers(teamSeparator);
            _playerFactory.CreateTankUiSpawners(_enemyFactory.EnemyDamageManagers);
            _playerFactory.CreateHud();
        }

        private void CreateEnemys(TeamSeparator teamSeparator)
        {
            _enemyFactory.CreateGameController();
            _enemyFactory.CreateEnemies(teamSeparator);
        }
    }
}