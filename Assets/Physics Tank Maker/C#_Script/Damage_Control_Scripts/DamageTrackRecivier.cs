using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using UnityEngine.Events;

public class TrackInfoHolder
{
    public float CurrentHP { get; private set; }
    public bool IsRightSide { get; private set; }
    public bool IsRestoring { get; private set; }

    private float _maxHP;

    public TrackInfoHolder(float maxHP, bool isRightSide)
    {
        _maxHP = maxHP;
        CurrentHP = _maxHP;
        IsRightSide = isRightSide;
    }
    public void DecreaseHP(float damage)
    {
        CurrentHP -= damage;
    }

    public bool IsBreach()
    {
        return CurrentHP <= 0;
    }

    public void RestoreHP()
    {
        Debug.Log("Restore");
        IsRestoring = false;
        CurrentHP = _maxHP;
    }

    internal void StartRestoring()
    {
        IsRestoring = true;
    }
}
public class DamageTrackRecivier : MonoBehaviour
{
    [SerializeField] private bool _isRepairable = true;
    [SerializeField] private List<TrackPieceDamage> _trackColliders;

    private TrackInfoHolder _rightTrack;
    private TrackInfoHolder _leftTrack;

    private float _repairDuration = 15;
    public bool IsRepaired => _isRepairable;
    public float RepairDuration => _repairDuration;

    [HideInInspector] public UnityEvent<TrackInfoHolder> OnTrackDestroyed;
    [HideInInspector] public UnityEvent<TrackInfoHolder> OnTrackRestored;
    public void Inititalize(TrackRecivierSettings recivierSettings)
    {
        _repairDuration = recivierSettings.RepairingDuration;

        _rightTrack = new TrackInfoHolder(recivierSettings.MaxHitPoints, true);
        _leftTrack = new TrackInfoHolder(recivierSettings.MaxHitPoints, false);

        foreach (var piece in _trackColliders)
        {
            piece.Initialize(recivierSettings);
            piece.OnDamaged.AddListener(CheckDamage);
        }
    }

    private void CheckDamage(float damage, TrackPieceDamage trackPiece)
    {
        TrackInfoHolder currentTrack = trackPiece.IsRightSide ? _rightTrack : _leftTrack;
        currentTrack.DecreaseHP(damage);
        if (currentTrack.IsBreach() && !currentTrack.IsRestoring)
        {
            print("BREACH");
            trackPiece.ParthDestroy();
            StartCoroutine(Track_Repairing_Timer(currentTrack, trackPiece));
            currentTrack.StartRestoring();
            OnTrackDestroyed?.Invoke(currentTrack);
            //траки копируются после разрушения
        }
    }

    private IEnumerator Track_Repairing_Timer(TrackInfoHolder currentTrack, TrackPieceDamage trackPiece)
    {
        var repairingTimer = 0.0f;
        while (repairingTimer < _repairDuration)
        {
            repairingTimer += Time.deltaTime;
            print(repairingTimer);
            // Set the HP.
            //currentTrack.CurrentHP = _initialHP * (repairingTimer / _repairDuration);

            yield return null;
        }
        OnTrackRestored?.Invoke(currentTrack);
        trackPiece.TrackRestore();
        currentTrack.RestoreHP();
    }

}
