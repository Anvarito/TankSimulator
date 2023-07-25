using System;
using System.Collections.Generic;
using Assets.Infrastructure.Services.Loading;
using Infrastructure.Assets;
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
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, ServiceLocator services, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, coroutineRunner, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader,services.Single<IAudioService>(), services.Single<IProgressService>(), services.Single<IStaticDataService>(), services.Single<IFactories>(), services.Single<ITrashRemoveService>(), services.Single<IScoreCounter>(), services.Single<ILoadingVisualizer>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IProgressService>(), services.Single<ISaveLoadService>()),
                [typeof(GameLoopState)] = new GameLoopState(this, services.Single<IInputService>(), services.Single<ITimerService>(), services.Single<IKillCounter>(), services.Single<IScoreCounter>(), services.Single<IProgressService>(), services.Single<IStaticDataService>(), services.Single<IFactories>()),
                [typeof(VictoryState)] = new VictoryState(this, coroutineRunner,services.Single<IAudioService>(),services.Single<ITimerService>(),services.Single<IInputService>(), services.Single<IFactories>(), services.Single<IProgressService>(), services.Single<ISaveLoadService>()),
                [typeof(DefeatState)] = new DefeatState(this, coroutineRunner, services.Single<IFactories>(), services.Single<ITimerService>(), services.Single<IProgressService>(), services.Single<IInputService>(), services.Single<ISaveLoadService>(), services.Single<IAudioService>()),
                [typeof(MenuState)] = new MenuState(this,sceneLoader,services.Single<IAudioService>(),services.Single<IProgressService>(),services.Single<ISaveLoadService>(),services.Single<IInputService>(),services.Single<IFactories>()),
                [typeof(SetupPlayersState)] = new SetupPlayersState(this,sceneLoader,services.Single<IInputService>(),services.Single<IFactories>(), services.Single<IProgressService>(), services.Single<INameRandomizer>()),
                [typeof(LaunchTrainingLevel)] = new LaunchTrainingLevel(this, services.Single<IInputService>(), services.Single<IStaticDataService>(),  services.Single<IProgressService>()),
                [typeof(SetupFirstInputState)] = new SetupFirstInputState(this,sceneLoader,services.Single<IProgressService>(),services.Single<IInputService>(),services.Single<IFactories>(), services.Single<IAudioService>()),
                [typeof(ChooseLevelModeState)] = new ChooseLevelModeState(this,sceneLoader,services.Single<IProgressService>(), services.Single<IInputService>(), services.Single<IFactories>()),
                [typeof(ResetState)] = new ResetState(this, services.Single<IFactories>(), services.Single<IInputService>(), services.Single<IProgressService>(), services.Single<ITimerService>(), services.Single<IKillCounter>()),
                [typeof(ReloadState)] = new ReloadState(this, services.Single<IKillCounter>(), services.Single<IFactories>(), services.Single<IInputService>(), sceneLoader, services.Single<IProgressService>(), services.Single<ITimerService>()),
                [typeof(PauseState)] = new PauseState(this,services.Single<ISaveLoadService>(),services.Single<IProgressService>(),services.Single<IAudioService>(),services.Single<ITimerService>(),services.Single<IInputService>() , services.Single<IFactories>()),
            };


        }


        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();

            LogState<TState>();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            IPayloadedState<TPayload> state = ChangeState<TState>();
            state.Enter(payload);
            
            LogState<TState>();
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }

        public bool InSetupPlayersState() =>
            _activeState is SetupPlayersState;

        public bool InSetupInputState() =>
            _activeState is SetupFirstInputState;

        private void LogState<TState>() where TState : class, IExitableState =>
            Debug.Log($"Entered {typeof(TState).Name}");
    }
}