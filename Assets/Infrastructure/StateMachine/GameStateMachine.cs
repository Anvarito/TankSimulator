using System;
using System.Collections.Generic;
using Infrastructure.Factory.Compose;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.SaveLoad;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, ServiceLocator serviceLocator, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, serviceLocator),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, serviceLocator.Single<IFactories>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this, serviceLocator.Single<IProgressService>(), serviceLocator.Single<ISaveLoadService>()), 
                [typeof(GameLoopState)] = new GameLoopState(this, coroutineRunner,serviceLocator.Single<IFactories>()),
                [typeof(VictoryState)] = new VictoryState(this, serviceLocator.Single<IFactories>()),
                [typeof(GameOverState)] = new GameOverState(this),
                [typeof(MenuState)] = new MenuState(this,sceneLoader,serviceLocator.Single<IInputService>(),serviceLocator.Single<IFactories>()),
                [typeof(SetupPlayersState)] = new SetupPlayersState(this,sceneLoader,serviceLocator.Single<IInputService>(),serviceLocator.Single<IFactories>()),
                [typeof(SetupFirstInputState)] = new SetupFirstInputState(this,sceneLoader,serviceLocator.Single<IInputService>(),serviceLocator.Single<IFactories>()),
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