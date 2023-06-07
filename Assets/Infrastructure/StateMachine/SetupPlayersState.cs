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
            _inputService.ConnectToInputs(tankPickerUI, individually: true);

            _inputFactory.TankPickerUIHelpers.Last().OnFirstTank += PickFirstTank;
            _inputFactory.TankPickerUIHelpers.Last().OnSecondTank += PickSecondTank;
        }

        private void PickSecondTank(Infrastructure.Services.Input.PlayerConfiguration playerConfiguration)
        {
            playerConfiguration.IsReady = true;
            playerConfiguration.TankIndex = 1;

            EnterNextStateIfReady();
        }

        private void PickFirstTank(Infrastructure.Services.Input.PlayerConfiguration playerConfiguration)
        {
            playerConfiguration.IsReady = true;
            playerConfiguration.TankIndex = 0;

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