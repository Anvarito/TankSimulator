using System.Collections;
using ChobiAssets.PTM;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class TurretDamageControlProp
{
    public Barrel_Base_CS Barel;
    public Cannon_Base_CS Canon;
    public Turret_Base_CS Turret;

    public bool blowOff;
    public float mass;
    public Particle_Control_CS destroyedEffect;
}
[System.Serializable]
public struct BodyExplosionProp
{
    public GameObject Destroyed_Effect;
    public Vector3 Destroyed_Effect_Offset;
}
public class TankExplosionVisual : MonoBehaviour
{
    [SerializeField] private DamageReciviersManager _damageReciviersManager;
    [SerializeField] private TurretDamageControlProp _turretProps;
    [SerializeField] private BodyExplosionProp _bodyExplosionProps;

    void Start()
    {
        _damageReciviersManager.OnTankDestroyed.AddListener(TankDestroy);
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

            Instantiate(_turretProps.destroyedEffect, _turretProps.Turret.TurretMesh.position, _turretProps.Turret.TurretMesh.rotation, _turretProps.Turret.TurretMesh);

        // Send Message to "Damage_Control_01_Turret_CS", "Turret_Horizontal_CS", "Cannon_Vertical_CS", "Cannon_Fire_CS", "Gun_Camera_CS", "Recoil_Brake_CS", "Sound_Control_Motor_CS".
        //_turretProps.turretBaseTransform.BroadcastMessage("Turret_Destroyed_Linkage", SendMessageOptions.DontRequireReceiver);

        // Blow off the turret.
        if (_turretProps.blowOff)
            BlowOffTurret();
    }

    private void BlowOffTurret()
    {
        _turretProps.Turret.TurretMesh.parent = null;
        _turretProps.Turret.TurretMesh.tag = Layer_Settings_CS.FinishTag;
        _turretProps.Turret.TurretMesh.gameObject.layer = 0;
        Rigidbody turretRigidbody = _turretProps.Turret.TurretMesh.gameObject.AddComponent<Rigidbody>();

        _turretProps.Barel.BarrelMesh.parent = turretRigidbody.transform;
        _turretProps.Barel.BarrelMesh.gameObject.layer = 0;
        _turretProps.Canon.CanonMesh.parent = turretRigidbody.transform;
        _turretProps.Canon.CanonMesh.gameObject.layer = 0;

        turretRigidbody.mass = _turretProps.mass;
        Vector3 addForceOffset;
        addForceOffset.x = Random.Range(-4.0f, 4.0f);
        addForceOffset.y = 0.0f;
        addForceOffset.z = Random.Range(-4.0f, 4.0f);
        turretRigidbody.AddForceAtPosition(_turretProps.Turret.TurretMesh.up * Random.Range(_turretProps.mass * 5.0f, _turretProps.mass * 15.0f), _turretProps.Turret.TurretMesh.position + addForceOffset, ForceMode.Impulse);
        _turretProps.Turret.TurretMesh.parent = null;
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
