using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Services.Score;
using Infrastructure.Services.Timer;
using UnityEngine;

public class RecivierUIManager : MonoBehaviour
{
    [Header("Reciviers")]
    [SerializeField] private AimMarkerUIReceiver _aimMarkerRecivier;
    [SerializeField] private LeadMarkerUIReceiver _leadMarkerRecivier;
    [SerializeField] private ReloadingCircleUIReseiver _reloadingRecivier;
    [SerializeField] private ReticleControlUIReceiver _reticleRecivier;
    [SerializeField] private HitPoitsBarUIReceiver _hitPointsRecivier;
    [SerializeField] private SpeedIndicatorRecivier _speedRecivier;
    [SerializeField] private PositionActorsMarkerRecivier _positionActorsRecivier;
    [SerializeField] private HitPointsTargetUIRecivier _hitPointsTargetRecivier;
    [SerializeField] private ScoreAndTimerReciever _scoreAndTimerReciever;

    internal void Initialize(Aiming_Control_CS aiming,
        Bullet_Generator_CS bulletGenerator,
        Cannon_Fire_CS cannonFire,
        Gun_Camera_CS gunCamera,
        DamageReceiversManager damageReceiver,
        Drive_Control_CS driveControl,
        CameraViewSetup cameraView,
        List<ID_Settings_CS> enemysID,
        ID_Settings_CS idSettings,
        ITimerService timerService,
        IScoreCounter scoreCounter,
        string playerName)
    {
        _aimMarkerRecivier.Init(aiming, gunCamera, cameraView);
        _leadMarkerRecivier.Init(aiming, bulletGenerator, gunCamera, cameraView);
        _reloadingRecivier.Init(cannonFire, gunCamera, cameraView);
        _reticleRecivier.Init(gunCamera, cameraView);
        _hitPointsRecivier.Init(damageReceiver, gunCamera, cameraView);
        _speedRecivier.Init(driveControl, gunCamera, cameraView);
        _positionActorsRecivier.Init(idSettings, enemysID, gunCamera, cameraView);
        _hitPointsTargetRecivier.Init(aiming, gunCamera, cameraView);
        _scoreAndTimerReciever.Init(timerService, scoreCounter, gunCamera, cameraView, idSettings);

        damageReceiver.OnTankDestroyed.AddListener(TankDestroyed);
    }

    public void AddNewEnemyToPositionActorsUI(ID_Settings_CS newBot)
    {
        _positionActorsRecivier.AddNewBot(newBot);
    }

    private void TankDestroyed(ID_Settings_CS tankId, ID_Settings_CS killerID)
    {
        _aimMarkerRecivier.PlayerDestoryed();
        _leadMarkerRecivier.PlayerDestoryed();
        _reloadingRecivier.PlayerDestoryed();
        _reticleRecivier.PlayerDestoryed();
        _hitPointsRecivier.PlayerDestoryed();
        _speedRecivier.PlayerDestoryed();
        _positionActorsRecivier.PlayerDestoryed();
        _hitPointsTargetRecivier.PlayerDestoryed();
        _scoreAndTimerReciever.PlayerDestoryed();
    }
}
