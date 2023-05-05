using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using UnityEngine.Events;

public class TrackDamageRecivier
{
    public float CurrentHP { get; private set; }
    public bool IsRightSide { get; private set; }
    public bool IsRestoring { get; private set; }
    public float MaxHP { get; private set; }

    private float _repairDuration;
    private List<DamageTrackPieceRecivier> trackPieceDamages = new List<DamageTrackPieceRecivier>();

    public UnityEvent<TrackDamageRecivier> OnTrackDamaged = new UnityEvent<TrackDamageRecivier>();
    public UnityEvent<TrackDamageRecivier> OnTrackBreached = new UnityEvent<TrackDamageRecivier>();
    public UnityEvent<TrackDamageRecivier> OnTrackRestored = new UnityEvent<TrackDamageRecivier>();

    public TrackDamageRecivier(TrackRecivierSettings recivierSettings, bool isRightSide)
    {
        MaxHP = recivierSettings.MaxHitPoints;
        _repairDuration = recivierSettings.RepairingDuration;
        CurrentHP = MaxHP;
        IsRightSide = isRightSide;
    }
    public void AddTrackPiece(DamageTrackPieceRecivier trackPieceDamage)
    {
        trackPieceDamages.Add(trackPieceDamage);
        trackPieceDamage.OnDamaged.AddListener(PieceDamaged);
    }

    private void PieceDamaged(float damage, DamageTrackPieceRecivier pieceDamage)
    {
        if (!IsBreach())
        {
            DecreaseHP(damage);
        }

        if (IsBreach() && !IsRestoring)
        {
            pieceDamage.GetSelfTrackPiece().Start_Breaking();
            pieceDamage.StartCoroutine(TrackRepairingProcess(pieceDamage));
            OnTrackBreached?.Invoke(this);
        }
    }

    private void DecreaseHP(float damage)
    {
        CurrentHP -= damage;
        OnTrackDamaged?.Invoke(this);
    }

    private bool IsBreach()
    {
        return CurrentHP <= 0;
    }

    private void RestoreHP()
    {
        Debug.Log("Restore");
        IsRestoring = false;
        CurrentHP = MaxHP;
    }

    private IEnumerator TrackRepairingProcess(DamageTrackPieceRecivier pieceDamage)
    {
        IsRestoring = true;
        var repairingTimer = 0.0f;
        while (repairingTimer < _repairDuration)
        {
            repairingTimer += Time.deltaTime;
            Debug.Log(repairingTimer);
            // Set the HP.
            //currentTrack.CurrentHP = _initialHP * (repairingTimer / _repairDuration);

            yield return null;
        }
        OnTrackRestored?.Invoke(this);
        pieceDamage.GetSelfTrackPiece().TrackRestore();
        RestoreHP();
    }
}
public class TracksHolder : MonoBehaviour
{
    [SerializeField] private bool _isRepairable = true;
    [SerializeField] private List<Drive_Wheel_Parent_CS> _driveWheels;

    public TrackDamageRecivier RightTrack { get; private set; }
    public TrackDamageRecivier LeftTrack { get; private set; }

    [HideInInspector] public UnityEvent<TrackDamageRecivier> OnTrackDestroyed;
    [HideInInspector] public UnityEvent<TrackDamageRecivier> OnTrackRestored;
    public void Inititalize(TrackRecivierSettings recivierSettings)
    {
        RightTrack = new TrackDamageRecivier(recivierSettings, true);
        LeftTrack = new TrackDamageRecivier(recivierSettings, false);

        foreach (Transform piece in transform)
        {
            if (piece.TryGetComponent(out DamageTrackPieceRecivier trackPieceDamage))
            {
                if (trackPieceDamage.IsRightSide)
                    RightTrack.AddTrackPiece(trackPieceDamage);
                else
                    LeftTrack.AddTrackPiece(trackPieceDamage);

                trackPieceDamage.Initialize(recivierSettings);
            }

        }

        RightTrack.OnTrackRestored.AddListener(TrackRestored);
        RightTrack.OnTrackBreached.AddListener(TrackDestroyed);

        LeftTrack.OnTrackRestored.AddListener(TrackRestored);
        LeftTrack.OnTrackBreached.AddListener(TrackDestroyed);
    }

    private void TrackDestroyed(TrackDamageRecivier track)
    {
        OnTrackDestroyed?.Invoke(track);

        foreach(var i in _driveWheels)
        {
            i.DriveBreak(track);
        }
    }

    private void TrackRestored(TrackDamageRecivier track)
    {
        OnTrackRestored?.Invoke(track);

        foreach (var i in _driveWheels)
        {
            i.DriveRun(track);
        }
    }
}
