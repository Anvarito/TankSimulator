using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using System;

public class HitPoitsBarUIReceiver : MonoBehaviour
{
    [SerializeField] private HitPointsUI _ui_HP_Bars_Self_CSprefab;
    [SerializeField] private TracksHolder _tracksHolder;

    private HitPointsUI _hitPointsUI;

    private bool _leftTrackBreach = false;
    private bool _rightTrackBreach = false;

    void Start()
    {
        _hitPointsUI = Instantiate(_ui_HP_Bars_Self_CSprefab);
        _hitPointsUI.Initialize();

        _tracksHolder.LeftTrack.OnTrackDamaged.AddListener(TrackDamaged);
        _tracksHolder.LeftTrack.OnTrackRestored.AddListener(TrackRestore);
        _tracksHolder.LeftTrack.OnTrackBreached.AddListener(TrackBreach);

        _tracksHolder.RightTrack.OnTrackDamaged.AddListener(TrackDamaged);
        _tracksHolder.RightTrack.OnTrackRestored.AddListener(TrackRestore);
        _tracksHolder.RightTrack.OnTrackBreached.AddListener(TrackBreach);
    }

    private void TrackDamaged(TrackDamageRecivier track)
    {
        _hitPointsUI.TrackDamageShow(track);
    }

    private void TrackRestore(TrackDamageRecivier track)
    {
       
    }

    private void TrackBreach(TrackDamageRecivier track)
    {
        if (track.IsRightSide)
            _rightTrackBreach = true;
        else
            _leftTrackBreach = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
