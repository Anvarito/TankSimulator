using System.Reflection;
using Infrastructure.Assets;
using Infrastructure.Factory;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Input;
using Infrastructure.Services.KillCounter;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.Score;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.Timer;

namespace Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ServiceLocator _services;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            ICoroutineRunner coroutineRunner, ServiceLocator services)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _coroutineRunner = coroutineRunner;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            ClearLog();

            _sceneLoader.Load(Initial, EnterLoadLevelState);

            // RegisterServices();
            _services.Single<IStaticDataService>().LoadAllStaticData();
            
            _services.Single<IFactories>().Single<IAudioFactory>().CreateMusicSource();
            _services.Single<IFactories>().Single<IAudioFactory>().CreateSoundSource();
        }

        public void Exit()
        {
        }

        private void EnterLoadLevelState() =>
            _gameStateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            
            _services.RegisterSingle<ITimerService>(new TimerService(_coroutineRunner));

            _services.RegisterSingle<IStaticDataService>(new StaticDataService());
            _services.Single<IStaticDataService>().LoadAllStaticData();

            _services.RegisterSingle<IProgressService>(new ProgressService());
            _services.RegisterSingle<IAssetLoader>(new AssetLoader());

            _services.RegisterSingle<IFactories>(new Factories());
            
            _services.Single<IFactories>().Add<IAudioFactory>((new AudioFactory(_services.Single<IAssetLoader>())));
            _services.RegisterSingle<IAudioService>(new AudioService(_services.Single<IFactories>(), _services.Single<IStaticDataService>()));
            
            _services.Single<IFactories>().Add<IInputFactory>(new InputFactory(_services.Single<IAudioService>(),_services.Single<IAssetLoader>()));
            _services.Single<IFactories>().Add<IEnemyFactory>(new EnemyFactory(_services.Single<IAudioService>(),_services.Single<IAssetLoader>(), _services.Single<IStaticDataService>(), _services.Single<IProgressService>()));
            _services.Single<IFactories>().Add<IWindowUIFactory>(new WindowUIFactory(_services.Single<IAudioService>(),_services.Single<IAssetLoader>()));

            _services.RegisterSingle<IInputService>(new InputService(_gameStateMachine,
                _services.Single<IFactories>(), _services.Single<IStaticDataService>()));

            _services.RegisterSingle<IScoreCounter>(new ScoreCounter(_services.Single<IFactories>(), _services.Single<IProgressService>(), _services.Single<IStaticDataService>()));

            _services.Single<IFactories>().Add<IPlayerFactory>(
                new PlayerFactory(
                    _services.Single<IAudioService>(),
                    _services.Single<IAssetLoader>(),
                    _services.Single<IInputService>(),
                    _services.Single<IProgressService>(),
                    _services.Single<IStaticDataService>(),
                    _services.Single<ITimerService>(),
                    _services.Single<IScoreCounter>()
                    ));

            _services.RegisterSingle<IKillCounter>(new KillCounter(_services.Single<IFactories>(), _services.Single<IProgressService>()));

            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IProgressService>(),
                _services.Single<IFactories>()));

            
            _services.RegisterSingle<ITrashRemoveService>(new TrashRemoveService());
            
        }

        private static void ClearLog()
        {
#if UNITY_EDITOR
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method?.Invoke(new object(), null);
#endif
        }
    }
}