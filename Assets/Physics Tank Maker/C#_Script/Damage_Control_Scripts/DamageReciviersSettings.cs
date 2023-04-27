using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RecivierSettings
{
    public float MaxHitPoints;
    [Range(0,1000)] public float DamageTreshold;
} 


[ CreateAssetMenu(fileName = "DamageReciviersSettings", menuName = "ScriptableObjects/DamageReciviersSettings", order = 1)]
public class DamageReciviersSettings : ScriptableObject
{
    [SerializeField] private RecivierSettings _turretDamageSettings;
    [SerializeField] private RecivierSettings _bodyDamageSettings;
    [SerializeField] private RecivierSettings _trackDamageSettings;

    public RecivierSettings GetTurretsettings()
    {
        return _turretDamageSettings;
    }

    public RecivierSettings GetBodysettings()
    {
        return _turretDamageSettings;
    }

    public RecivierSettings GetTracksettings()
    {
        return _turretDamageSettings;
    }
}
