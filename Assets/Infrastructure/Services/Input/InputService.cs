using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Assets;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.StateMachine;
using Infrastructure.TestMono;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


namespace Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        public Action OnPlayerJoined { get; set; }
        public List<PlayerConfiguration> PlayerConfigs { get; } = new List<PlayerConfiguration>();

        private readonly GameStateMachine _gameStateMachine;

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

        public void ConnectToInputs(GameObject uiInputModule, bool individually = false)
        {
            if (individually)
            {
                if (NextPlayerExist()) ResetPlayerIndex();
                PlayerConfigs[currentIndex].Input.uiInputModule = uiInputModule.GetComponentInChildren<InputSystemUIInputModule>();
                uiInputModule.GetComponentInChildren<TankPickerUIHelper>().Construct(PlayerConfigs[currentIndex]);
                currentIndex += 1;
            }
            else
            {
                foreach (PlayerConfiguration playerConfig in PlayerConfigs)
                {
                    playerConfig.Input.uiInputModule = uiInputModule.GetComponentInChildren<InputSystemUIInputModule>();
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
                PlayerConfigs.Add(new PlayerConfiguration(pi));
                pi.transform.parent = _inputManager.transform;
                OnPlayerJoined?.Invoke();
                Debug.Log("Player joined");
            }
        }

        private bool CanAddPlayer() =>
            (_gameStateMachine.InSetupInputState() && PlayerConfigs.Count < 1) ||
            (_gameStateMachine.InSetupPlayersState() && PlayerConfigs.Count < 2);

        private bool NextPlayerExist() => 
            currentIndex + 1 > PlayerConfigs.Count;
    }


    public class PlayerConfiguration
    {
        public PlayerInput Input { get; private set; }
        public int PlayerIndex { get; private set; }

        public bool IsReady { get; set; }
        
        public int TankIndex { get; set; }

        public PlayerConfiguration(PlayerInput pi)
        {
            PlayerIndex = pi.playerIndex;
            Input = pi;
        }
    }
}