using System.Collections;
using ChobiAssets.PTM;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct TurretDamageControlProp
{
    public DamageTurret DamageTurret;

    public Transform Barel;
    public Transform Canon;
    public Transform Turret;

    public bool blowOff;
    public float mass;
    public GameObject destroyedEffect;
}
[System.Serializable]
public struct BodyExplosionProp
{
    public DamageBodyRecivier DamageBodyRecivier;
    public GameObject Destroyed_Effect;
    public Vector3 Destroyed_Effect_Offset;
}
public class TankExplosionVisual : MonoBehaviour
{
    [SerializeField] private TurretDamageControlProp _turretProps;
    [SerializeField] private BodyExplosionProp _bodyExplosionProps;

    void Start()
    {
        _turretProps.DamageTurret.OnDestroyed.AddListener(TankDestroy);
        _bodyExplosionProps.DamageBodyRecivier.OnDestroyed.AddListener(TankDestroy);
    }

    private void TankDestroy()
    {
        TurretDestroyProcess();
        BodyDestroyProcess();
    }

    private void BodyDestroyProcess()
    {
        CreateDestroyedEffect();
        CreateNavMeshObstacle();
        StartCoroutine(Disable_MainBody_Constraints());
    }

    private void TurretDestroyProcess()
    {
        // Create the destroyed effect.
        if (_turretProps.destroyedEffect)
        {
            Instantiate(_turretProps.destroyedEffect, _turretProps.Turret.position, _turretProps.Turret.rotation, _turretProps.Turret);
        }

        // Send Message to "Damage_Control_01_Turret_CS", "Turret_Horizontal_CS", "Cannon_Vertical_CS", "Cannon_Fire_CS", "Gun_Camera_CS", "Recoil_Brake_CS", "Sound_Control_Motor_CS".
        //_turretProps.turretBaseTransform.BroadcastMessage("Turret_Destroyed_Linkage", SendMessageOptions.DontRequireReceiver);

        // Blow off the turret.
        if (_turretProps.blowOff)
            BlowOffTurret();
    }

    private void BlowOffTurret()
    {
        _turretProps.Turret.parent = null;
        Rigidbody turretRigidbody = _turretProps.Turret.gameObject.AddComponent<Rigidbody>();

        _turretProps.Barel.parent = turretRigidbody.transform;
        _turretProps.Canon.parent = turretRigidbody.transform;

        turretRigidbody.mass = _turretProps.mass;
        Vector3 addForceOffset;
        addForceOffset.x = Random.Range(-4.0f, 4.0f);
        addForceOffset.y = 0.0f;
        addForceOffset.z = Random.Range(-4.0f, 4.0f);
        turretRigidbody.AddForceAtPosition(_turretProps.Turret.up * Random.Range(_turretProps.mass * 5.0f, _turretProps.mass * 15.0f), _turretProps.Turret.position + addForceOffset, ForceMode.Impulse);
        _turretProps.Turret.parent = null;
        // Change the hierarchy.
        //Turret_Props[index].turretBaseTransform.parent = bodyTransform.parent; // Make it a child of the top object.
    }

    private void CreateDestroyedEffect()
    {
        var Destroyed_Effect = _bodyExplosionProps.Destroyed_Effect;
        var Destroyed_Effect_Offset = _bodyExplosionProps.Destroyed_Effect_Offset;

        if (Destroyed_Effect)
        {
            Vector3 pos = transform.position + (transform.right * Destroyed_Effect_Offset.x) + (transform.up * Destroyed_Effect_Offset.y) + (transform.forward * Destroyed_Effect_Offset.z);
            Instantiate(Destroyed_Effect, pos, transform.rotation, transform);
        }
    }

    private void CreateNavMeshObstacle()
    {
        NavMeshObstacle navMeshObstacle = transform.gameObject.AddComponent<NavMeshObstacle>();
        navMeshObstacle.shape = NavMeshObstacleShape.Capsule;
        navMeshObstacle.carving = true;
        navMeshObstacle.carvingMoveThreshold = 1.0f;
    }

    IEnumerator Disable_MainBody_Constraints()
    {
        // Disable the rigidbody constraints in the MainBody in order to release the parking brake.
        yield return new WaitForFixedUpdate(); // This wait is required for PhysX.
        Rigidbody bodyRigidBody = transform.GetComponent<Rigidbody>();
        bodyRigidBody.constraints = RigidbodyConstraints.None;
    }
}
