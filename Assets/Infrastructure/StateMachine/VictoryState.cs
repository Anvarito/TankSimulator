using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class VictoryState : IPayloadedState<float>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPlayerFactory _playerFactory;
        private readonly IInputService _inputService;

        public VictoryState(GameStateMachine gameStateMachine,IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _playerFactory = factories.Single<IPlayerFactory>();
        }
        
        public void Enter(float score)
        {
            Debug.Log($"Entered {this.GetType().Name}");
            
            _playerFactory.GameBoard.ShowVictoryPanel(score);
            _playerFactory.GameBoard.OnPressContinue += Menu;

            SetupMenu();
        }

        private void SetupMenu()
        {
            //_inputService.ConnectToInputs(_playerFactory.GameBoard.transform.root.gameObject, true);

        }

        private void Menu() => 
            _gameStateMachine.Enter<MenuState>();

        public void Exit()
        {
            _playerFactory.GameBoard.OnPressContinue -= Menu;
        }
    }
}