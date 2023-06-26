using ChobiAssets.PTM;
using UnityEngine;

public class HitPointsTargetUIRecivier : UIRecivierBase
{
    [SerializeField] private LeadMarkerUIReceiver _leadMarkerResivier;
    private HitPointsTargetUIPresenter _hitPointsTargetUI;
    private EActiveCameraType _currentCameraType;

    private Aiming_Control_CS _aimingControl;
    private DamageReceiversManager _targetDamageReceivers;
    private Transform _targetTransform;
    public void Init(Aiming_Control_CS aimingControl, Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup)
    {
        _gunCamera = gunCamera;
        _cameraSetup = cameraSetup;

        _aimingControl = aimingControl;

        InitialUIRecivier();
    }

    protected override void InstantiateCanvas()
    {
        base.InstantiateCanvas();
        _hitPointsTargetUI = _spawnedPresenter as HitPointsTargetUIPresenter;
        _hitPointsTargetUI.SetCamera(_cameraSetup.GetCamera());
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
        Transform aimTransform = _currentCameraType == EActiveCameraType.GunCamera ?  _leadMarkerResivier.GetTargetTransform() : _aimingControl.TargetAimHook;
        if (aimTransform != null)
        {
            if (_targetTransform != aimTransform)
            {
                if (_targetDamageReceivers)
                    Unsubscribe();

                _hitPointsTargetUI.Show(aimTransform);

                _targetTransform = aimTransform;
                _targetDamageReceivers = aimTransform.GetComponentInParent<DamageReceiversManager>();

                Subcribe();

                float alphaBodyHP = _targetDamageReceivers.BodyDamageRecivier.CurentHP / _targetDamageReceivers.BodyDamageRecivier.MaxHP;
                float alphaTurretHP = _targetDamageReceivers.TurretDamageRecivier.CurentHP / _targetDamageReceivers.TurretDamageRecivier.MaxHP;
                float alphaRTrackHP = _targetDamageReceivers.TrackDamageRecivier.RightTrack.CurrentHP / _targetDamageReceivers.TrackDamageRecivier.RightTrack.MaxHP;
                float alphaLTrackHP = _targetDamageReceivers.TrackDamageRecivier.LeftTrack.CurrentHP / _targetDamageReceivers.TrackDamageRecivier.LeftTrack.MaxHP;

                _hitPointsTargetUI.Refresh(alphaBodyHP, alphaTurretHP, alphaRTrackHP, alphaLTrackHP);
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
        _targetDamageReceivers.OnTurretDamaged.AddListener(TurretDamaged);
        _targetDamageReceivers.OnBodyDamaged.AddListener(BodyDamaged);

        _targetDamageReceivers.OnTrackDamaged.AddListener(TrackDamaged);
        _targetDamageReceivers.OnTrackRestore.AddListener(TrackRestore);
        _targetDamageReceivers.OnTrackBreach.AddListener(TrackBreach);
    }

    private void Unsubscribe()
    {
        _targetDamageReceivers.OnTurretDamaged.RemoveListener(TurretDamaged);
        _targetDamageReceivers.OnBodyDamaged.RemoveListener(BodyDamaged);

        _targetDamageReceivers.OnTrackDamaged.RemoveListener(TrackDamaged);
        _targetDamageReceivers.OnTrackRestore.RemoveListener(TrackRestore);
        _targetDamageReceivers.OnTrackBreach.RemoveListener(TrackBreach);
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
