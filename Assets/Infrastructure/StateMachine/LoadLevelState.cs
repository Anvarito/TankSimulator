using Infrastructure.Factory;
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

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
        }

        public void Enter(string payload)
        {
            _sceneLoader.Load(name: payload, OnLoaded);
        }

        private void OnLoaded()
        {
            _gameFactory.CreatePlayer(at: GameObject.FindWithTag(PlayerInitialPoint));
            _gameFactory.CreateHud();
            _gameFactory.CreateEnemies(at: GameObject.FindGameObjectsWithTag(EnemyInitialPoint));
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }
    }
}