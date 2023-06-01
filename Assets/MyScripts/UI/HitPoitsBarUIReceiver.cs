using UnityEngine;
using ChobiAssets.PTM;

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

    protected override void SwitchCamera(EActiveCameraType activeCamera)
    {
        base.SwitchCamera(activeCamera);
        _hitPointsUI.SetCamera(activeCamera == EActiveCameraType.GunCamera ? _cameraSetup.GetGunCamera() : _cameraSetup.GetCamera());
    }

    public void Initialize(DamageReciviersManager partsDamageReceiver)
    {
        _damageManager = partsDamageReceiver;
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
