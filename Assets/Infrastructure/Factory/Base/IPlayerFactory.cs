using System;
using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Components;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.TestMono;
using UnityEngine;

namespace Infrastructure.Factory.Base
{
    public interface IPlayerFactory : IGameFactory
    {
        List<PlayerUiParts> PlayerParts { get; }
        GameOverBoard GameBoard { get; }
        void CreatePlayers(List<SpawnPointConfig> points);
        void CreateTankUiSpawners(List<DamageReceiversManager> enemyDamageList);
        void CreateHud();

        MainMenuUIHelper MainMenuUIHelper { get; }
        Action OnPlayerDestroyed { get; set; }
        int PlayerCount { get; }
        List<ID_Settings_CS> EnemysID { get; }
        List<ID_Settings_CS> PlayersSettings { get; }
        GameObject CreateMainMenu();
        GamemodeMapHelper CreateMapModeChoiseUI();
        void AddNewEnemyToPositionActorsUI(ID_Settings_CS newEnemy);
    }
}