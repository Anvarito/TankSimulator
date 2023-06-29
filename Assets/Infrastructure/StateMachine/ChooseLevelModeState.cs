using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData.Gamemodes;
using Infrastructure.Services.StaticData.Level;

namespace Infrastructure.StateMachine
{
    public class ChooseLevelModeState : IState
    {
        private const string LevelModeChoise = "LevelModeChoise";

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

        public void Enter() =>
            _sceneLoader.Load(LevelModeChoise, OnLoad);

        public void Exit()
        {
        }

        private void OnLoad()
        {
            var helper = _playerFactory.CreateMapModeChoiseUI();
            helper.OnContinueClick += Continue;
        }

        private void Continue()
        {
            SetupPlayerTeams();
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.Level);
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
            IsVersusMode() || IsDeathMatchMode();

        private bool IsDeathMatchMode() => 
            _progressService.Progress.WorldData.ModeId == GamemodeId.DeathMatch;

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