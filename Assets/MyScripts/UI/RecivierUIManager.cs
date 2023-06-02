using System;
using System.Collections;
using System.Collections.Generic;
using ChobiAssets.PTM;
using UnityEngine;

[System.Serializable]
public struct ColorsHolder
{
    public Color mainColor;
    public Color secondaryColor;
}
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

    [Space(10)]
    [SerializeField] private ColorsHolder _colorsHolder;

    private DamageReciviersManager _damageRecivierManager;
    private Gun_Camera_CS _gunCamera;
    private CameraViewSetup _cameraSetup;
    private Aiming_Control_CS _aiming_Control;
    private ID_Settings_CS _selfID;
    private Cannon_Fire_CS _cannonFire;
    private Bullet_Generator_CS _bulletGenerator;
    private Drive_Control_CS _driveControl;
    private List<ID_Settings_CS> _enemysID;
    private ID_Settings_CS _idSettings;

    internal void Initialize(Aiming_Control_CS aiming,
        Bullet_Generator_CS bulletGenerator,
        Cannon_Fire_CS cannonFire,
        Gun_Camera_CS gunCamera,
        DamageReciviersManager damageReceiver,
        Drive_Control_CS driveControl,
        CameraViewSetup cameraView,
        List<ID_Settings_CS> enemysID,
        ID_Settings_CS idSettings)
    {
        _aiming_Control = aiming;
        _cameraSetup = cameraView;
        _damageRecivierManager = damageReceiver;
        _gunCamera = gunCamera;
        _cannonFire = cannonFire;
        _bulletGenerator = bulletGenerator;
        _driveControl = driveControl;
        _enemysID = enemysID;
        _idSettings = idSettings;

        _damageRecivierManager.OnTankDestroyed.AddListener(TankDestroyed);

        InitAllReciviers();
    }

    private void TankDestroyed(ID_Settings_CS killerID)
    {
        _aimMarkerRecivier.PlayerDestoryed();
        _leadMarkerRecivier.PlayerDestoryed();
        _reloadingRecivier.PlayerDestoryed();
        _reticleRecivier.PlayerDestoryed();
        _hitPointsRecivier.PlayerDestoryed();
        _speedRecivier.PlayerDestoryed();
        _positionActorsRecivier.PlayerDestoryed();
        _hitPointsTargetRecivier.PlayerDestoryed();
    }

    private void InitAllReciviers()
    {
        _aimMarkerRecivier.Init(_aiming_Control, _gunCamera, _cameraSetup);
        _leadMarkerRecivier.Init(_aiming_Control, _bulletGenerator, _gunCamera, _cameraSetup);
        _reloadingRecivier.Init(_cannonFire, _gunCamera, _cameraSetup);
        _reticleRecivier.Init(_gunCamera, _cameraSetup);
        _hitPointsRecivier.Init(_damageRecivierManager, _gunCamera, _cameraSetup);
        _speedRecivier.Init(_driveControl, _gunCamera, _cameraSetup);
        _positionActorsRecivier.Init(_idSettings, _enemysID, _gunCamera, _cameraSetup);
        _hitPointsTargetRecivier.Init(_aiming_Control, _gunCamera, _cameraSetup);
    }
}
