using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
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
        private readonly IPlayerFactory _playerFactory;

        public ChooseLevelModeState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            IProgressService progressService, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _progressService = progressService;
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
            }
            else
            {
                SetPlayerInSameTeam();
            }
        }

        private void SetPlayerInSameTeam() =>
            _progressService.Progress.WorldData.Teams =
                Enumerable.Range(1, 2).Select(x => ERelationship.TeamA).ToList();

        private void SetPlayersInDifferentTeams() =>
            _progressService.Progress.WorldData.Teams = new List<ERelationship>()
            {
                ERelationship.TeamA,
                ERelationship.TeamB
            };

        private bool IsPLayersInDifferentTeams() => 
            IsVersusMode() || IsDeathMatchMode();

        private bool IsDeathMatchMode() => 
            _progressService.Progress.WorldData.ModeId == GamemodeId.DeathMatch;

        private bool IsVersusMode() => 
            _progressService.Progress.WorldData.ModeId == GamemodeId.Versus;
    }
}