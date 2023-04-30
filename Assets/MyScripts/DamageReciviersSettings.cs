using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RecivierSettings
{
    public float MaxHitPoints;
    [Range(0,1000)] public float DamageTreshold;
}
[System.Serializable]
public class TrackRecivierSettings : RecivierSettings
{
    [Range(0, 20)] public float RepairingDuration;
}

[ CreateAssetMenu(fileName = "DamageReciviersSettings", menuName = "ScriptableObjects/DamageReciviersSettings", order = 1)]
public class DamageReciviersSettings : ScriptableObject
{
    [SerializeField] private RecivierSettings _turretDamageSettings;
    [SerializeField] private RecivierSettings _bodyDamageSettings;
    [SerializeField] private TrackRecivierSettings _trackDamageSettings;

    public RecivierSettings GetTurretsettings()
    {
        return _turretDamageSettings;
    }

    public RecivierSettings GetBodysettings()
    {
        return _bodyDamageSettings;
    }

    public TrackRecivierSettings GetTracksettings()
    {
        return _trackDamageSettings;
    }
}
