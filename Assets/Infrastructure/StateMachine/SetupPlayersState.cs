using Infrastructure.Factory.Compose;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class SetupPlayersState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public SetupPlayersState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string sceneName)
        {
            Debug.Log($"Entered {this.GetType().Name}");
            
            _sceneLoader.Load(sceneName, onLoad);
        }

        public void Exit()
        {
            
        }

        private void onLoad()
        {
            
        }
    }
}