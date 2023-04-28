using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageble
{
    public void DealDamage(float damage, int bulletType);
    public bool CheckBreackout(float damage, int bulletType);
}
