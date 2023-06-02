using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameOverState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public GameOverState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Exit()
        {
            
        }

        public void Enter()
        {
            Debug.Log($"Entered {this.GetType().Name}");   
        }
    }
}