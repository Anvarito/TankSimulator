namespace Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private const string Main = "Main";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(Initial, EnterLoadLevelState);
            RegisterServices();
        }

        public void Exit()
        {
        }

        private void RegisterServices()
        {
        }

        private void EnterLoadLevelState() =>
            _gameStateMachine.Enter<LoadLevelState, string>(Main);
    }
}