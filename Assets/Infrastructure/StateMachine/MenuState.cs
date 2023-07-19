using System;
using System.Linq;
using Infrastructure.Components;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.StateMachine
{
    public class MenuState : IState
    {
        private const string MainMenu = "MainMenu";
        private const string SetupTankMain = "SetupTankRoman";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private InfoScrolling _infoScrolling;
        private readonly IAudioService _audioService;
        private readonly IProgressService _progress;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IInputService _inputService;
        private readonly IProgressService _progressService;
        private readonly IPlayerFactory _playerFactory;

        public MenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IAudioService audioService, IProgressService progress, ISaveLoadService saveLoadService, IInputService inputService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _audioService = audioService;
            _progress = progress;
            _saveLoadService = saveLoadService;
            _inputService = inputService;
            _playerFactory = factories.Single<IPlayerFactory>();
        }

        public void Enter() =>
            _sceneLoader.Load(MainMenu, onLoad);

        public void Exit()
        {
            UnregisterButtonsEvents(_playerFactory.MainMenuUIHelper);
            _inputService.PlayerConfigs.First().Input.onActionTriggered -= OnScroll;
        }

        private void onLoad() =>
            SetupMenu();

        private void SetupMenu()
        {
            _playerFactory.CreateMainMenu();
            RegisterButtonsEvents(_playerFactory.MainMenuUIHelper);
            SetupVolumeSliders();
            _inputService.ConnectToInputs(_playerFactory.MainMenuUIHelper.gameObject);

            _inputService.PlayerConfigs.First().Input.onActionTriggered += OnScroll;
        }

        private void OnScroll(InputAction.CallbackContext input)
        {
            if (input.action.name == _inputService.Control.TankMovement.Look.name)
            {
                if (input.performed)
                    _infoScrolling.ScrollMove(input.ReadValue<Vector2>().y);
                else if (input.canceled)
                    _infoScrolling.ScrollMove(0);
            }
        }

        private void SetupVolumeSliders()
        {
            _playerFactory.MainMenuUIHelper.MusicSlider.value = _progress.Progress.WorldData.MusicVolume;
            _playerFactory.MainMenuUIHelper.SoundsSlider.value = _progress.Progress.WorldData.SoundsVolume;
        }

        private void RegisterButtonsEvents(MainMenuUIHelper mainMenu)
        {
            //mainMenu.OnContinueButtonPress.AddListener(ContinueGame);
            mainMenu.OnOnNewGameButtonPress.AddListener(StartNewGame);
            mainMenu.OnTrainButtonPress.AddListener(TrainLaunch);
            mainMenu.OnExitButtonPress.AddListener(ExitGame);
            mainMenu.OnMusicSlider.AddListener(ChangeMusicVolume);
            mainMenu.OnSoundsSlider.AddListener(ChangeSoundsVolume);
            _infoScrolling = mainMenu.InfoScrolling;
        }



        private void UnregisterButtonsEvents(MainMenuUIHelper mainMenu)
        {
            //mainMenu.OnContinueButtonPress.RemoveListener(ContinueGame);
            mainMenu.OnOnNewGameButtonPress.RemoveListener(StartNewGame);
            mainMenu.OnTrainButtonPress.RemoveListener(TrainLaunch);
            mainMenu.OnExitButtonPress.RemoveListener(ExitGame);
            mainMenu.OnMusicSlider.RemoveListener(ChangeMusicVolume);
            mainMenu.OnSoundsSlider.RemoveListener(ChangeSoundsVolume);
        }

        private void ContinueGame()
        {
        }
        private void TrainLaunch()
        {
            _gameStateMachine.Enter<LaunchTrainingLevel>();
        }
        private void ExitGame()
        {
            Debug.Log("Quit game");
            Application.Quit();
        }
        private void StartNewGame()
        {
            _saveLoadService.SaveProgress();
            _gameStateMachine.Enter<SetupPlayersState, string>(SetupTankMain);
        }

        private void ChangeMusicVolume(float volume)
        {
            _audioService.ChangeMusicVolume(volume);
            _progress.Progress.WorldData.MusicVolume = volume;
        }

        private void ChangeSoundsVolume(float volume)
        {
            _audioService.ChangeSoundVolume(volume);
            _progress.Progress.WorldData.SoundsVolume = volume;
        }
    }
}