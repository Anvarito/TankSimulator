using ChobiAssets.PTM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointsTargetUIRecivier : UIRecivierBase
{
    [SerializeField] private HitPointsTargetUIPresenter _hitPointsTargetUIprefab;
    [SerializeField] private LeadMarkerUIReceiver _leadMarkerResivier;
    private HitPointsTargetUIPresenter _hitPointsTargetUI;
    private EActiveCameraType _currentCameraType;

    private DamageReciviersManager _targetDamageReciviers;
    private Transform _targetTransform;
    private ERelationship _targetRelationship;
    protected override void InstantiateCanvas()
    {
        base.InstantiateCanvas();
        _hitPointsTargetUI = Instantiate(_hitPointsTargetUIprefab);
        _hitPointsTargetUI.InitialCanvas(_hitPointsTargetUI.GetComponent<Canvas>(), _cameraSetup.GetCamera());
        _hitPointsTargetUI.Hide();
    }

    protected override void SwitchCamera(EActiveCameraType activeCamera)
    {
        base.SwitchCamera(activeCamera);
        _currentCameraType = activeCamera;
        _hitPointsTargetUI.SetCamera(activeCamera == EActiveCameraType.GunCamera ? _cameraSetup.GetGunCamera() : _cameraSetup.GetCamera());
    }

    private void Update()
    {
        var aimTransform = _currentCameraType == EActiveCameraType.GunCamera ?  _leadMarkerResivier.GetTargetTransform() : _aimingControl.TargetAimHook;
        if (aimTransform != null)
        {
            if (_targetTransform != aimTransform)
            {
                if (_targetDamageReciviers)
                    Unsubscribe();

                _targetTransform = aimTransform;
                _targetDamageReciviers = aimTransform.GetComponentInParent<DamageReciviersManager>();
                _targetRelationship = _targetTransform.GetComponentInParent<ID_Settings_CS>().Relationship;

                Subcribe();

                _hitPointsTargetUI.Show(_targetTransform, _targetRelationship == _IDSettings.Relationship);

                float alphaBodyHP = _targetDamageReciviers.BodyDamageRecivier.CurentHP / _targetDamageReciviers.BodyDamageRecivier.MaxHP;
                float alphaTurretHP = _targetDamageReciviers.TurretDamageRecivier.CurentHP / _targetDamageReciviers.TurretDamageRecivier.MaxHP;
                float alphaRTrackHP = _targetDamageReciviers.TrackDamageRecivier.RightTrack.CurrentHP / _targetDamageReciviers.TrackDamageRecivier.RightTrack.MaxHP;
                float alphaLTrackHP = _targetDamageReciviers.TrackDamageRecivier.LeftTrack.CurrentHP / _targetDamageReciviers.TrackDamageRecivier.LeftTrack.MaxHP;

                _hitPointsTargetUI.Refresh(alphaBodyHP, alphaTurretHP, alphaRTrackHP, alphaLTrackHP);
            }
            else
            {
                _hitPointsTargetUI.Show(_targetTransform, _targetRelationship == _IDSettings.Relationship);
            }
        }
        else
        {
            if (_targetTransform != null)
            {
                _targetTransform = null;
                _hitPointsTargetUI.Hide();
                Unsubscribe();
            }
        }
    }

    private void Subcribe()
    {
        _targetDamageReciviers.OnTurretDamaged.AddListener(TurretDamaged);
        _targetDamageReciviers.OnBodyDamaged.AddListener(BodyDamaged);

        _targetDamageReciviers.OnTrackDamaged.AddListener(TrackDamaged);
        _targetDamageReciviers.OnTrackRestore.AddListener(TrackRestore);
        _targetDamageReciviers.OnTrackBreach.AddListener(TrackBreach);
    }

    private void Unsubscribe()
    {
        _targetDamageReciviers.OnTurretDamaged.RemoveListener(TurretDamaged);
        _targetDamageReciviers.OnBodyDamaged.RemoveListener(BodyDamaged);

        _targetDamageReciviers.OnTrackDamaged.RemoveListener(TrackDamaged);
        _targetDamageReciviers.OnTrackRestore.RemoveListener(TrackRestore);
        _targetDamageReciviers.OnTrackBreach.RemoveListener(TrackBreach);
    }

    private void TrackBreach(TrackDamageRecivier arg0)
    {
        _hitPointsTargetUI.TrackBreached(arg0);
    }

    private void TrackRestore(TrackDamageRecivier arg0)
    {
        _hitPointsTargetUI.TrackRestored(arg0);
    }

    private void TrackDamaged(TrackDamageRecivier arg0)
    {
        _hitPointsTargetUI.TrackDamageShow(arg0);
    }

    private void BodyDamaged(float arg0, float arg1)
    {
        _hitPointsTargetUI.BodyDamageShow(arg0, arg1);
    }

    private void TurretDamaged(float arg0, float arg1)
    {
        _hitPointsTargetUI.TurretDamageShow(arg0, arg1);
    }

    protected override void DestroyUI()
    {
    }
}
