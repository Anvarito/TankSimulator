using Infrastructure.Services;
using Infrastructure.StateMachine;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine GameStateMachine;
        private ICoroutineRunner _coroutineRunner;

        public Game(ICoroutineRunner coroutineRunner)
        {
            GameStateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), ServiceLocator.Container);
        }
    }
}