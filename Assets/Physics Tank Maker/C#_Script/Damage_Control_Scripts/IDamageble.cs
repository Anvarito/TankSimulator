using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ChobiAssets.PTM;
public interface IDamageble
{
    public float DamageTreshold { get; }
    public void DealDamage(float damage, ID_Settings_CS bulletLauncherID);
    public bool CheckBreackout(float damage);
}
