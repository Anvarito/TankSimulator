using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Progress;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class MenuState : IState
    {
        private const string MainMenu = "MainMenu";
        private const string SetupTankMain = "SetupTankRoman";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IProgressService _progressService;
        private readonly IPlayerFactory _playerFactory;


        public MenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IProgressService progressService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
            _playerFactory = factories.Single<IPlayerFactory>();
        }

        public void Enter()
        {
            Debug.Log($"Entered {this.GetType().Name}");
            
            _sceneLoader.Load(MainMenu, onLoad);
        }

        public void Exit() => 
            UnregisterButtonsEvents(_playerFactory.MainMenuUIHelper);

        private void onLoad() =>
            SetupMenu();

        private void SetupMenu()
        {
            _playerFactory.CreateMainMenu();
            RegisterButtonsEvents(_playerFactory.MainMenuUIHelper);
        }

        private void RegisterButtonsEvents(MainMenuUIHelper mainMenu)
        {
            mainMenu.OnContinueButtonPress.AddListener(ContinueGame);
            mainMenu.OnOnNewGameButtonPress.AddListener(StartNewGame);
        }

        private void UnregisterButtonsEvents(MainMenuUIHelper mainMenu)
        {
            mainMenu.OnContinueButtonPress.RemoveListener(ContinueGame);
            mainMenu.OnOnNewGameButtonPress.RemoveListener(StartNewGame);
        }

        private void ContinueGame()
        {
            Debug.Log("Continue button pressed");
            // if (_progressService.Progress.StateType is GameLoopState)
            _gameStateMachine.Enter<LoadLevelState,string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        private void StartNewGame() => 
            _gameStateMachine.Enter<SetupPlayersState,string>(SetupTankMain);
    }
}