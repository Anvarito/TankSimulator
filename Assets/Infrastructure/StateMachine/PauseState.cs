using Infrastructure.Factory;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.Timer;

namespace Infrastructure.StateMachine
{
    public class PauseState : IState
    {
        private const string ReloadScene = "ReloadScene";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProgressService _progress;
        private readonly IAudioService _audioService;
        private readonly ITimerService _timerService;
        private readonly IInputService _inputService;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IWindowUIFactory _windowFactory;
        private PauseMenu _pauseMenu;

        public PauseState(GameStateMachine gameStateMachine,ISaveLoadService saveLoadService,IProgressService progress,IAudioService audioService,ITimerService timerService, IInputService inputService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
            _progress = progress;
            _audioService = audioService;
            _timerService = timerService;
            _inputService = inputService;
            _enemyFactory = factories.Single<IEnemyFactory>();
            _windowFactory = factories.Single<IWindowUIFactory>();
        }

        public void Enter()
        {
            _audioService.PauseMusic();
            _enemyFactory.Controller.Pause();
            _timerService.PauseTimer();
            
            _pauseMenu = _windowFactory.CreatePauseMenu();
            SetupPauseMenu();

            _inputService.OnEscTriggered += UnpauseGame;
        }

        public void Exit()
        {
            _audioService.PauseMusic();
            _enemyFactory.Controller.Pause();
            _timerService.PauseTimer();
            
            // _saveLoadService.SaveProgress();
            
            Unsubscribe();
            _windowFactory.DestroyPauseMenu();
            
            _inputService.OnEscTriggered -= UnpauseGame;
        }

        private void SetupPauseMenu()
        {
            _inputService.ResetPlayerIndex();
            _inputService.ConnectToInputs(_pauseMenu.gameObject, individually: true);
            
            _pauseMenu.MusicSlider.value = _progress.Progress.WorldData.MusicVolume;
            _pauseMenu.SoundSlider.value = _progress.Progress.WorldData.SoundsVolume;
            
            Subscribe();
        }

        private void Subscribe()
        {
            _pauseMenu.OnContinueClick += UnpauseGame;
            _pauseMenu.OnRestartClick += RestartGame;
            _pauseMenu.OnExitClick += ExitToMenu;

            _pauseMenu.OnMusicVolumeChange += HandleMusicVolumeChange;
            _pauseMenu.OnSoundVolumeChange += HandleSoundsVolumeChange;
        }

        private void Unsubscribe()
        {
            _pauseMenu.OnContinueClick -= UnpauseGame;
            _pauseMenu.OnRestartClick -= RestartGame;
            _pauseMenu.OnExitClick -= ExitToMenu;

            _pauseMenu.OnMusicVolumeChange -= HandleMusicVolumeChange;
            _pauseMenu.OnSoundVolumeChange -= HandleSoundsVolumeChange;
        }

        private void HandleSoundsVolumeChange(float volume)
        {
            _progress.Progress.WorldData.SoundsVolume = volume;
            _audioService.ChangeSoundVolume(volume);
        }

        private void HandleMusicVolumeChange(float volume)
        {
            _progress.Progress.WorldData.MusicVolume = volume;
            _audioService.ChangeMusicVolume(volume);
        }

        private void UnpauseGame() => 
            _gameStateMachine.Enter<GameLoopState>();

        private void RestartGame()
        {
            _audioService.StopMusic();
            _gameStateMachine.Enter<ReloadState, string>(ReloadScene);
        }

        private void ExitToMenu()
        {
            _audioService.StopMusic();
            _gameStateMachine.Enter<ResetState>();
        }
    }
}