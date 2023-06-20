using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Services.Progress;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.TestMono;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Factory.Base
{
    public interface IGameFactory : IFactory
    {
        List<IProgressWriter> ProgressWriters { get; }
        List<IProgressReader> ProgressReaders { get; }
        void CleanUp();
    }

    public interface IPlayerFactory : IGameFactory
    {
        List<PlayerUiParts> PlayerParts { get; }
        GameOverBoard GameBoard { get; }

        void CreatePlayers(List<SpawnPointConfig> points);
        void CreateTankUiSpawners(List<DamageReciviersManager> enemyDamageList);
        void CreateHud();


        MainMenuUIHelper MainMenuUIHelper { get; }
        GameObject CreateMainMenu();
        GamemodeMapHelper CreateMapModeChoiseUI();
    }

    public interface IEnemyFactory : IGameFactory
    {
        List<DamageReciviersManager> EnemyDamageManagers { get; }
        public void CreateGameController();
        void CreateEnemy(SpawnPointConfig config);
    }
    
    internal interface IInputFactory : IGameFactory
    {
        List<TankPickerUIHelper> TankPickerUIHelpers { get; }
        PlayerInputManager PlayerInputManager { get; }
        GameObject CreatePlayerInputManager();
        void CretePleasePressButtonPanel();
        GameObject CreateTankPickerUI(Transform parent);
        Transform CreatePickerCanvas();
    }
}