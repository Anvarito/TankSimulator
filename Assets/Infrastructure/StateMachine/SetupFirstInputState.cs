using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;

namespace Infrastructure.StateMachine
{
    public class SetupFirstInputState : IState
    {
        private const string Intro = "Intro";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IProgressService _progress;
        private readonly IInputService _inputService;
        private readonly IAudioService _audioService;
        private readonly IInputFactory _inputFactory;

        public SetupFirstInputState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,IProgressService progress,IInputService inputService, IFactories factories, IAudioService audioService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _progress = progress;
            _inputService = inputService;
            _audioService = audioService;
            _inputFactory = factories.Single<IInputFactory>();
        }

        public void Enter()
        {
            _sceneLoader.Load(Intro,onLoad);
            _inputService.OnPlayerJoined += EnterChooseLevelModeState;
        }


        public void Exit()
        {
            _sceneLoader.CancelLoading();
            _inputService.OnPlayerJoined -= EnterChooseLevelModeState;
        }

        private void EnterChooseLevelModeState() => 
            _gameStateMachine.Enter<MenuState>();
        private void onLoad()
        {
            _audioService.ChangeMusicVolume(_progress.Progress.WorldData.MusicVolume);
            _audioService.ChangeSoundVolume(_progress.Progress.WorldData.SoundsVolume);
            
            _audioService.PlayMusic(MusicId.MenuRockCalm);
            
            _inputFactory.CretePleasePressButtonPanel();
        }
    }
}