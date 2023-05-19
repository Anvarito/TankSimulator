using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class SetupFirstInputState : IState
    {
        private const string Intro = "Intro";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IInputService _inputService;
        private readonly IInputFactory _inputFactory;

        public SetupFirstInputState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,IInputService inputService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _inputService = inputService;
            _inputFactory = factories.Single<IInputFactory>();
        }

        public void Enter()
        {
            Debug.Log($"Entered {this.GetType().Name}");
            
            _sceneLoader.Load(Intro,onLoad);
            _inputService.OnPlayerJoined += () => 
                _gameStateMachine.Enter<LoadProgressState>();
        }

        public void Exit() => 
            _sceneLoader.CancelLoading();

        private void onLoad() => 
            _inputFactory.CretePleasePressButtonPanel();
    }
}