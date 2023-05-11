using Infrastructure.Factory;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string PlayerInitialPoint = "PlayerInitialPoint";
        private const string EnemyInitialPoint = "EnemyInitialPoint";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IProgressService _progressService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory, IProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter(string payload)
        {
            Debug.Log($"Entered {this.GetType().Name}");
            
            _gameFactory.CleanUp();
            _sceneLoader.Load(name: payload, OnLoaded);
        }

        private void OnLoaded()
        {
            InitGameLevel();
            InformProgressReaders();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }

        private void InformProgressReaders()
        {
            foreach (IProgressReader progress in _gameFactory.ProgressReaders) 
                progress.LoadProgress(_progressService.Progress);
        }

        private void InitGameLevel()
        {
            _gameFactory.CreatePlayer(at: GameObject.FindWithTag(PlayerInitialPoint));
            _gameFactory.CreateHud();
            _gameFactory.CreateEnemies(at: GameObject.FindGameObjectsWithTag(EnemyInitialPoint));
        }
    }
}