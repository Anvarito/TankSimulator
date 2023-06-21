using System;
using System.Collections.Generic;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticData;

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
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, services.Single<IFactories>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IProgressService>(), services.Single<ISaveLoadService>()), 
                [typeof(GameLoopState)] = new GameLoopState(this, coroutineRunner,services.Single<IFactories>()),
                [typeof(VictoryState)] = new VictoryState(this, services.Single<IFactories>()),
                [typeof(GameOverState)] = new GameOverState(this, services.Single<IFactories>(), services.Single<IProgressService>()),
                [typeof(MenuState)] = new MenuState(this,sceneLoader,services.Single<IInputService>(),services.Single<IFactories>()),
                [typeof(SetupPlayersState)] = new SetupPlayersState(this,sceneLoader,services.Single<IInputService>(),services.Single<IFactories>()),
                [typeof(SetupFirstInputState)] = new SetupFirstInputState(this,sceneLoader,services.Single<IInputService>(),services.Single<IFactories>()),
                [typeof(ChooseLevelModeState)] = new ChooseLevelModeState(this,sceneLoader,services.Single<IProgressService>(), services.Single<IFactories>()),
                [typeof(ResetState)] = new ResetState(this, services.Single<IFactories>(), services.Single<IInputService>(), services.Single<IProgressService>()),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState,TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            IPayloadedState<TPayload> state = ChangeState<TState>();
            state.Enter(payload);
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
    }
}