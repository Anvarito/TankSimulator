using UnityEngine;
using ChobiAssets.PTM;

public class HitPoitsBarUIReceiver : MonoBehaviour
{
    [SerializeField] private HitPointsUI _ui_HP_Bars_Self_CSprefab;
    [SerializeField] private DamageReciviersManager _damageManager;

    private HitPointsUI _hitPointsUI;


    void Start()
    {
        _hitPointsUI = Instantiate(_ui_HP_Bars_Self_CSprefab);
        _hitPointsUI.Initialize();

        _damageManager.OnTurretDamaged.AddListener(TurretDamaged);
        _damageManager.OnBodyDamaged.AddListener(BodyDamaged);

        _damageManager.OnTrackDamaged.AddListener(TrackDamaged);
        _damageManager.OnTrackRestore.AddListener(TrackRestore);
        _damageManager.OnTrackBreach.AddListener(TrackBreach);
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
}
