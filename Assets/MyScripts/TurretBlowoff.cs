using System.Collections;
using System.Collections.Generic;
using ChobiAssets.PTM;
using UnityEngine;

[System.Serializable]
public struct TurretDamageControlProp
{
    public Transform turretBaseTransform;
    public bool blowOff;
    public float mass;
    public GameObject destroyedEffect;
}
public class TurretBlowoff : MonoBehaviour
{
    [SerializeField] private TurretDamageControlProp _turretProps;
    private DamageTurret _damageTurret;
    void Start()
    {
        _damageTurret = GetComponent<DamageTurret>();
        _damageTurret.OnDestroyed.AddListener(TurretDestroy);
    }

    private void TurretDestroy()
    {
        // Create the destroyed effect.
        if (_turretProps.destroyedEffect)
        {
            Instantiate(_turretProps.destroyedEffect, _turretProps.turretBaseTransform.position, _turretProps.turretBaseTransform.rotation, _turretProps.turretBaseTransform);
        }

        // Send Message to "Damage_Control_01_Turret_CS", "Turret_Horizontal_CS", "Cannon_Vertical_CS", "Cannon_Fire_CS", "Gun_Camera_CS", "Recoil_Brake_CS", "Sound_Control_Motor_CS".
        //_turretProps.turretBaseTransform.BroadcastMessage("Turret_Destroyed_Linkage", SendMessageOptions.DontRequireReceiver);

        // Blow off the turret.
        if (_turretProps.blowOff == true)
        {
            BlowOffTurret();
        }

        if (_turretProps.blowOff)
            BlowOffTurret();
    }

    private void BlowOffTurret()
    {
        Rigidbody turretRigidbody = _turretProps.turretBaseTransform.gameObject.AddComponent<Rigidbody>();
        turretRigidbody.mass = _turretProps.mass;
        Vector3 addForceOffset;
        addForceOffset.x = Random.Range(-2.0f, 2.0f);
        addForceOffset.y = 0.0f;
        addForceOffset.z = Random.Range(-2.0f, 2.0f);
        turretRigidbody.AddForceAtPosition(_turretProps.turretBaseTransform.up * Random.Range(_turretProps.mass * 5.0f, _turretProps.mass * 15.0f), _turretProps.turretBaseTransform.position + addForceOffset, ForceMode.Impulse);
        _turretProps.turretBaseTransform.parent = null;
        // Change the hierarchy.
        //Turret_Props[index].turretBaseTransform.parent = bodyTransform.parent; // Make it a child of the top object.
    }
}
