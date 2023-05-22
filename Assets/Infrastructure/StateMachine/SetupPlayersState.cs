using System;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace Infrastructure.StateMachine
{
    public class SetupPlayersState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IInputService _inputService;
        private readonly IInputFactory _inputFactory;

        public SetupPlayersState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IInputService inputService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _inputService = inputService;
            _inputFactory = factories.Single<IInputFactory>();
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
            Transform canvas = _inputFactory.CreatePickerCanvas();
            _inputService.ResetPlayerIndex();
            
            CreatePicker(canvas);
            _inputService.OnPlayerJoined += () =>
                CreatePicker(canvas);
        }

        private void CreatePicker(Transform canvas)
        {
            GameObject tankPickerUI = _inputFactory.CreateTankPickerUI(canvas);
            InputSystemUIInputModule inputModule = tankPickerUI.GetComponentInChildren<InputSystemUIInputModule>();
            _inputService.ConnectToInputs(inputModule,individually: true);
        }

    }
}