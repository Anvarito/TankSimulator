using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using Infrastructure.Services.KillCounter;
using Infrastructure.Services.Progress;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class ReloadState : IPayloadedState<string>
    {

        private SceneLoader _sceneLoader;
        private GameStateMachine _gameStateMachine;
        private readonly IKillCounter _killCounter;
        private IPlayerFactory _playerFactory;
        private IEnemyFactory _enemyFactory;
        private IInputFactory _inputFactory;
        private IInputService _inputService;
        private IProgressService _progressService;
        public ReloadState(GameStateMachine gameStateMachine,IKillCounter killCounter, IFactories factories, IInputService inputService, SceneLoader sceneLoader, IProgressService progressService)
        {
            _sceneLoader = sceneLoader;

            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
            _inputFactory = factories.Single<IInputFactory>();

            _inputService = inputService;
            _progressService = progressService;

            _gameStateMachine = gameStateMachine;
            _killCounter = killCounter;
        }
        
        public void Enter(string sceneName)
        {
            _playerFactory.CleanUp();
            _enemyFactory.CleanUp();
            _inputFactory.CleanUp();
            _killCounter.CleanUp();
            
            //_inputService.CleanUp();
            _progressService.CleanUp();

            _sceneLoader.Load(sceneName, OnLoad);
        }

        public void Exit()
        {

        }

        private void OnLoad() => 
            _sceneLoader.Load(_progressService.Progress.WorldData.Level, LoadGameScene, OnLoadProgressShow);

        private void OnLoadProgressShow(float progress) => 
            Debug.Log(progress);

        private void LoadGameScene() => 
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.Level);
    }
}
