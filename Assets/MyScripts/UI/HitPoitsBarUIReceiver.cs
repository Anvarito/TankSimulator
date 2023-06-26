using ChobiAssets.PTM;

public class HitPoitsBarUIReceiver : UIRecivierBase
{

    private HitPointsUIPresenter _hitPointsUI;
    private DamageReceiversManager _damageManager;
    protected override void Subscribes()
    {
        base.Subscribes();
        _damageManager.OnTurretDamaged.AddListener(TurretDamaged);
        _damageManager.OnBodyDamaged.AddListener(BodyDamaged);

        _damageManager.OnTrackDamaged.AddListener(TrackDamaged);
        _damageManager.OnTrackRestore.AddListener(TrackRestore);
        _damageManager.OnTrackBreach.AddListener(TrackBreach);
    }
    public void Init(DamageReceiversManager damageManager, Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup)
    {
        _damageManager = damageManager;
        _gunCamera = gunCamera;
        _cameraSetup = cameraSetup;

        InitialUIRecivier();
    }
    protected override void InstantiateCanvas()
    {
        base.InstantiateCanvas();
        _hitPointsUI = _spawnedPresenter as HitPointsUIPresenter;
    }

    protected override void SwitchCamera(EActiveCameraType activeCamera)
    {
        base.SwitchCamera(activeCamera);
        _hitPointsUI.SetCamera(activeCamera == EActiveCameraType.GunCamera ? _cameraSetup.GetGunCamera() : _cameraSetup.GetCamera());
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
