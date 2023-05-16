using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class VictoryState : IPayloadedState<float>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _playerFactory;

        public VictoryState(GameStateMachine gameStateMachine,IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _playerFactory = factories.Single<IPlayerFactory>();
        }
        
        public void Enter(float score)
        {
            Debug.Log($"Entered {this.GetType().Name}");
            
            _playerFactory.GameBoard.ShowVictory(score);
            _playerFactory.GameBoard.OnPressContinue += Continue;
        }

        private void Continue() => 
            _gameStateMachine.Enter<BootstrapState>();

        public void Exit()
        {
            _playerFactory.GameBoard.OnPressContinue -= Continue;
        }
    }
}