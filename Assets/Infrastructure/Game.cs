using Infrastructure.Factory.Compose;
using Infrastructure.Services;
using Infrastructure.StateMachine;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine GameStateMachine;

        public Game(ICoroutineRunner coroutineRunner)
        {
            ServiceLocator serviceLocator = ServiceLocator.Container;
            GameStateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), serviceLocator, coroutineRunner);
        }
    }
}