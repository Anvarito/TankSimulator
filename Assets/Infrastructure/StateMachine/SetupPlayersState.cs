using System.Linq;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.StateMachine
{
    public class SetupPlayersState : IPayloadedState<string>
    {
        private const string LevelName = "MinimalTest";

        private readonly GameStateMachine _gameStateMachine;
        private readonly INameRandomizer _nameRandomizer;
        private readonly IProgressService _progressService;
        private readonly SceneLoader _sceneLoader;
        private readonly IInputService _inputService;
        private readonly IInputFactory _inputFactory;

        private Transform _canvas;

        public SetupPlayersState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IInputService inputService,
            IFactories factories, IProgressService progressService, INameRandomizer nameRandomizer)
        {
            _gameStateMachine = gameStateMachine;
            _nameRandomizer = nameRandomizer;
            _progressService = progressService;
            _sceneLoader = sceneLoader;
            _inputService = inputService;
            _inputFactory = factories.Single<IInputFactory>();
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, onLoad);
        }

        private void OnBack(InputAction.CallbackContext obj)
        {
            if(obj.performed && obj.action.name == "ReturnMenu")
            {
                _inputService.ResetToDefault();
                _gameStateMachine.Enter<MenuState>();
            }
        }

        public void Exit()
        {
            foreach (var i in _inputFactory.TankPickerUIHelpers)
                i.OnTankChoise.RemoveListener(PickTank);

            _inputService.PlayerConfigs.First().Input.onActionTriggered -= OnBack;
            _inputService.OnPlayerJoined -= CreatePicker;
        }

        private void onLoad()
        {
            _canvas = _inputFactory.CreatePickerCanvas();
            _inputService.ResetToDefault();
            _progressService.CleanUp();
            CreatePicker();
            
            _inputService.OnPlayerJoined += CreatePicker;
            _inputService.PlayerConfigs.First().Input.onActionTriggered += OnBack;
        }


        private void CreatePicker()
        {
            GameObject tankPickerUI = _inputFactory.CreateTankPickerUI(_canvas);
            _inputService.ConnectToInputs(tankPickerUI, individually: true);
            _inputService.PlayerConfigs.Last().PlayerName = _nameRandomizer.GetRandomedName();

            _inputFactory.TankPickerUIHelpers.Last().OnTankChoise.AddListener(PickTank);
            _inputFactory.TankPickerUIHelpers.Last().SetPlayerName(_inputService.PlayerConfigs.Last().PlayerName);
        }

        private void PickTank()
        {
            EnterNextStateIfReady();
        }

        private void EnterNextStateIfReady()
        {
            if (AllReady()) _gameStateMachine.Enter<ChooseLevelModeState>();
        }

        private bool AllReady() =>
            _inputService.PlayerConfigs.All(x => x.IsReady);
    }
}