using System.Reflection;
using Infrastructure.Assets;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ServiceLocator _services;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, ServiceLocator services)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            ClearLog();
            Debug.Log($"Entered {this.GetType().Name}");
            
            _sceneLoader.Load(Initial, EnterLoadLevelState);
            // RegisterServices();
        }

        public void Exit()
        {
        }

        private void EnterLoadLevelState() =>
            _gameStateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            _services.RegisterSingle<IProgressService>(new ProgressService());
            _services.RegisterSingle<IAssetLoader>( new AssetLoader());
            _services.RegisterSingle<IInputService>(new InputService());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetLoader>()));
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IProgressService>(),
                _services.Single<IGameFactory>()));
        }

        private static void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method?.Invoke(new object(), null);
        }
    }
}