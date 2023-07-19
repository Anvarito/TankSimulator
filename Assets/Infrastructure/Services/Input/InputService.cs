using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Components;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


namespace Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        public Action OnPlayerJoined { get; set; }
        public Action OnEscTriggered { get; set; }
        public List<PlayerConfiguration> PlayerConfigs { get; } = new List<PlayerConfiguration>();

        private readonly GameStateMachine _gameStateMachine;

        private StaticData.IStaticDataService _staticDataService;
        private readonly IInputFactory _inputFactory;
        private readonly Transform _choseCanvas;
        private PlayerInputManager _inputManager;
        private static int currentIndex = 0;
        private NewControl _controlSchema;

        public NewControl Control => _controlSchema;
        public InputService(GameStateMachine gameStateMachine, IFactories factories, StaticData.IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _inputFactory = factories.Single<IInputFactory>();
            _staticDataService = staticDataService;

            var inputManager = _inputFactory.CreatePlayerInputManager();
            _inputManager = inputManager.GetComponent<PlayerInputManager>();
            _inputManager.onPlayerJoined += HandlePlayerJoin;
            SetupControlSchema();
        }

        public void ConnectToInputs(GameObject uiInputModule, bool individually = false)
        {
            if (individually)
            {
                if (NextPlayerExist()) ResetPlayerIndex();
                PlayerConfigs[currentIndex].Input.uiInputModule = uiInputModule.GetComponentInChildren<InputSystemUIInputModule>();

                if (uiInputModule.TryGetComponent(out TankPickerUIHelper tankPickerUIHelper))
                {
                    tankPickerUIHelper.Construct(PlayerConfigs[currentIndex], _staticDataService);
                }

                //uiInputModule.GetComponentInChildren<TankPickerUIHelper>().Construct(PlayerConfigs[currentIndex], _staticDataService);
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

        private void SetupControlSchema()
        {
            _controlSchema = new NewControl();
            _controlSchema.Enable();
            SubscribeEscAction();
        }

        private void SubscribeEscAction() =>
            _controlSchema.TankMovement.Esc.performed += TriggerEscAction;

        private void TriggerEscAction(InputAction.CallbackContext context)
        {
            if (context.performed) OnEscTriggered?.Invoke();
        }


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

        public void CleanUp()
        {
            foreach (var i in PlayerConfigs)
            {
                GameObject.Destroy(i.Input.gameObject);
            }

            ResetPlayerIndex();

            PlayerConfigs.Clear();
        }

        public void ResetToDefault()
        {
            if (PlayerConfigs.Count > 1)
            {
                GameObject.Destroy(PlayerConfigs[1].Input.gameObject);
                PlayerConfigs.RemoveAt(1);
            }

            foreach(var i in PlayerConfigs)
            {
                i.IsReady = false;
            }

            ResetPlayerIndex();
        }
    }


    public class PlayerConfiguration
    {
        public PlayerInput Input { get; private set; }
        public int PlayerIndex { get; private set; }

        public bool IsReady { get; set; }

        public string PrefabPath { get; set; }
        public ERelationship Team { get; set; }

        public PlayerConfiguration(PlayerInput pi)
        {
            PlayerIndex = pi.playerIndex;
            Input = pi;
        }
    }
}