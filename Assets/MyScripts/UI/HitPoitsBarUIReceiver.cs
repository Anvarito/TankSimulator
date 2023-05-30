using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using System;

public class HitPoitsBarUIReceiver : UIRecivierBase
{

    private HitPointsUIPresenter _hitPointsUI;

    protected override void Subscribes()
    {
        base.Subscribes();
        _damageRecivierManager.OnTurretDamaged.AddListener(TurretDamaged);
        _damageRecivierManager.OnBodyDamaged.AddListener(BodyDamaged);

        _damageRecivierManager.OnTrackDamaged.AddListener(TrackDamaged);
        _damageRecivierManager.OnTrackRestore.AddListener(TrackRestore);
        _damageRecivierManager.OnTrackBreach.AddListener(TrackBreach);
    }

    protected override void InstantiateCanvas()
    {
        base.InstantiateCanvas();
        _hitPointsUI = Instantiate(_presenterPrefab) as HitPointsUIPresenter;
        _hitPointsUI.InitialCanvas();
        _hitPointsUI.SetCamera(_cameraSetup.GetCamera());
    }
    
    private void BodyDamaged(float currentHP, float maxHP)
    {
        _hitPointsUI.BodyDamageShow(currentHP, maxHP);
    }

    private void TurretDamaged(float currentHP, float maxHP)
    {
        _hitPointsUI.TurretDamageShow(currentHP, maxHP);
    }

    private void TrackDamaged(TrackDamageRecivier track)
    {
        _hitPointsUI.TrackDamageShow(track);
    }

    private void TrackRestore(TrackDamageRecivier track)
    {
        _hitPointsUI.TrackRestored(track);
    }

    private void TrackBreach(TrackDamageRecivier track)
    {
        _hitPointsUI.TrackBreached(track);
    }

    protected override void DestroyUI()
    {
        Destroy(_hitPointsUI.gameObject);
    }
}
