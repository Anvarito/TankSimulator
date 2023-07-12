using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData.Gamemodes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.StateMachine
{
    public class ChooseLevelModeState : IState
    {
        private const string LevelModeChoise = "LevelModeChoise";
        private const string SetupTankMain = "SetupTankRoman";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IProgressService _progressService;
        private readonly IInputService _inputService;
        private readonly IPlayerFactory _playerFactory;

        public ChooseLevelModeState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            IProgressService progressService, IInputService inputService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
            _inputService = inputService;
            _playerFactory = factories.Single<IPlayerFactory>();

        }

        private void OnBack(InputAction.CallbackContext obj)
        {
            if (obj.performed && obj.action.name == "ReturnMenu")
            {
                Back();
            }
        }

        public void Enter()
        {
            _sceneLoader.Load(LevelModeChoise, OnLoad);
            _inputService.PlayerConfigs.First().Input.onActionTriggered += OnBack;
        }

        public void Exit()
        {
            _inputService.PlayerConfigs.First().Input.onActionTriggered -= OnBack;
        }

        private void OnLoad()
        {
            var helper = _playerFactory.CreateMapModeChoiseUI();
            helper.OnContinueClick += Continue;
            helper.OnBackClick += Back;
        }

        private void Continue()
        {
            SetupPlayerTeams();
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.Level);
        }

        private void Back()
        {
            _gameStateMachine.Enter<SetupPlayersState, string>(SetupTankMain);
        }

        private void SetupPlayerTeams()
        {
            if (IsPLayersInDifferentTeams())
            {
                SetPlayersInDifferentTeams();
                SetPlayersInDifferentTeamsProgress();
            }
            else
            {
                SetPlayerInSameTeam();
                SetPlayerInSameTeamProgress();
            }
        }

        private void SetPlayerInSameTeam() =>
            _inputService.PlayerConfigs.ForEach(x => x.Team = ERelationship.TeamA);

        private void SetPlayersInDifferentTeams()
        {
            _inputService.PlayerConfigs.First().Team = ERelationship.TeamB;
            _inputService.PlayerConfigs.Last().Team = ERelationship.TeamA;
        }

        private bool IsPLayersInDifferentTeams() =>
            IsVersusMode();

        private bool IsVersusMode() =>
            _progressService.Progress.WorldData.ModeId == GamemodeId.Versus;


        private void SetPlayerInSameTeamProgress() =>
            _progressService.Progress.WorldData.Teams =
                Enumerable.Range(1, 2).Select(x => ERelationship.TeamA).ToList();

        private void SetPlayersInDifferentTeamsProgress() =>
            _progressService.Progress.WorldData.Teams = new List<ERelationship>()
            {
                ERelationship.TeamA,
                ERelationship.TeamB
            };
    }
}