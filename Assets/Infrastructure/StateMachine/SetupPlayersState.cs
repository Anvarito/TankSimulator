using System.Linq;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class SetupPlayersState : IPayloadedState<string>
    {
        private const string LevelName = "MinimalTest";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IInputService _inputService;
        private readonly IInputFactory _inputFactory;
        Transform _canvas;
        public SetupPlayersState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IInputService inputService,
            IFactories factories)
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
            _inputFactory.TankPickerUIHelpers.Last().OnTankChoise.RemoveListener(PickTank);
            _inputService.OnPlayerJoined -= CreatePicker;
        }

        private void onLoad()
        {
            _canvas = _inputFactory.CreatePickerCanvas();
            _inputService.ResetPlayerIndex();

            CreatePicker();
            _inputService.OnPlayerJoined += CreatePicker;
        }


        private void CreatePicker()
        {
            GameObject tankPickerUI = _inputFactory.CreateTankPickerUI(_canvas);
            _inputService.ConnectToInputs(tankPickerUI, individually: true);

            _inputFactory.TankPickerUIHelpers.Last().OnTankChoise.AddListener(PickTank);
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