using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Progress;

namespace Infrastructure.StateMachine
{
    public class ChooseLevelModeState : IState
    {
        private const string LevelModeChoise = "LevelModeChoise";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IProgressService _progressService;
        private readonly IPlayerFactory _playerFactory;

        public ChooseLevelModeState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, IProgressService progressService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
            _playerFactory = factories.Single<IPlayerFactory>();
        }

        public void Enter() => 
            _sceneLoader.Load(LevelModeChoise, OnLoad);

        public void Exit()
        {
        }

        private void OnLoad()
        {
            var helper = _playerFactory.CreateMapModeChoiseUI();
            helper.OnContinueClick += Continue;
        }

        private void Continue() => 
            _gameStateMachine.Enter<LoadLevelState,string>(_progressService.Progress.WorldData.Level);
    }
}