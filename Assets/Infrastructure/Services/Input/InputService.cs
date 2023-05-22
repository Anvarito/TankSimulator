using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


namespace Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        public Action OnPlayerJoined { get; set; }

        private readonly GameStateMachine _gameStateMachine;
        private List<PlayerConfiguration> _playerConfigs = new List<PlayerConfiguration>();

        private readonly IInputFactory _inputFactory;
        private readonly Transform _choseCanvas;
        private PlayerInputManager _inputManager;
        private static int currentIndex = 0;

        public InputService(GameStateMachine gameStateMachine, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _inputFactory = factories.Single<IInputFactory>();

            var inputManager = _inputFactory.CreatePlayerInputManager();
            _inputManager = inputManager.GetComponent<PlayerInputManager>();
            _inputManager.onPlayerJoined += HandlePlayerJoin;
        }

        public void ConnectToInputs(InputSystemUIInputModule uiInputModule, bool individually = false)
        {
            if (individually)
            {
                if (NextPlayerExist()) ResetPlayerIndex();
                _playerConfigs[currentIndex].Input.uiInputModule = uiInputModule;
                currentIndex += 1;
            }
            else
            {
                foreach (PlayerConfiguration playerConfig in _playerConfigs)
                {
                    playerConfig.Input.uiInputModule = uiInputModule;
                }
            }
        }

        public void ResetPlayerIndex() => 
            currentIndex = 0;


        private void HandlePlayerJoin(PlayerInput pi)
        {
            Debug.Log("Player try join" + pi.playerIndex);

            if (!CanAddPlayer())
            {
                GameObject.Destroy(pi.gameObject);
                Debug.Log("Player not joined");
            }
            else
            {
                _playerConfigs.Add(new PlayerConfiguration(pi));
                pi.transform.parent = _inputManager.transform;
                OnPlayerJoined?.Invoke();
                Debug.Log("Player joined");
            }
        }

        private bool CanAddPlayer() =>
            (_gameStateMachine.InSetupInputState() && _playerConfigs.Count < 1) ||
            (_gameStateMachine.InSetupPlayersState() && _playerConfigs.Count < 2);

        private bool NextPlayerExist() => 
            currentIndex + 1 > _playerConfigs.Count;
    }


    public class PlayerConfiguration
    {
        public PlayerInput Input { get; private set; }
        public int PlayerIndex { get; private set; }
        public bool isReady { get; set; }
        public int TankIndex { get; set; }

        public PlayerConfiguration(PlayerInput pi)
        {
            PlayerIndex = pi.playerIndex;
            Input = pi;
        }
    }
}