using Infrastructure.Components;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;

namespace Infrastructure.StateMachine
{
    public class MenuState : IState
    {
        private const string MainMenu = "MainMenu";
        private const string SetupTankMain = "SetupTankRoman";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IAudioService _audioService;
        private readonly IProgressService _progress;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IInputService _inputService;
        private readonly IProgressService _progressService;
        private readonly IPlayerFactory _playerFactory;


        public MenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,IAudioService audioService,IProgressService progress, ISaveLoadService saveLoadService, IInputService inputService, IFactories factories)
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

        public void Exit() => 
            UnregisterButtonsEvents(_playerFactory.MainMenuUIHelper);

        private void onLoad() =>
            SetupMenu();

        private void SetupMenu()
        {
            _playerFactory.CreateMainMenu();
            RegisterButtonsEvents(_playerFactory.MainMenuUIHelper);
            SetupVolumeSliders();
            _inputService.ConnectToInputs(_playerFactory.MainMenuUIHelper.gameObject);
            
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
            mainMenu.OnMusicSlider.AddListener(ChangeMusicVolume);
            mainMenu.OnSoundsSlider.AddListener(ChangeSoundsVolume);
        }

        private void UnregisterButtonsEvents(MainMenuUIHelper mainMenu)
        {
            //mainMenu.OnContinueButtonPress.RemoveListener(ContinueGame);
            mainMenu.OnOnNewGameButtonPress.RemoveListener(StartNewGame);
            mainMenu.OnMusicSlider.RemoveListener(ChangeMusicVolume);
            mainMenu.OnSoundsSlider.RemoveListener(ChangeSoundsVolume);
        }

        private void ContinueGame()
        {
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