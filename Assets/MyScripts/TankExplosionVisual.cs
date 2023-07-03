using System.Collections;
using ChobiAssets.PTM;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[System.Serializable]
public class TurretDamageControlProp
{
    public Barrel_Base_CS Barel;
    public Cannon_Base_CS Canon;
    public Turret_Base_CS Turret;

    public bool blowOff;
    public float mass;
    public Particle_Control_CS DestroyedEffect;
}
[System.Serializable]
public class BodyExplosionProp
{
    public ParticleSystem SmokeEffectPrefab;
    public ParticleSystem Destroyed_Effect;
    public Vector3 Destroyed_Effect_Offset;
}
public class TankExplosionVisual : MonoBehaviour
{
    [FormerlySerializedAs("damageReceiviersManager")] [FormerlySerializedAs("_damageReciviersManager")] [SerializeField] private DamageReceiversManager damageReceiversManager;
    [SerializeField] private TurretDamageControlProp _turretProps;
    [SerializeField] private BodyExplosionProp _bodyExplosionProps;
    void Start()
    {
        damageReceiversManager.OnTankDestroyed.AddListener(TankDestroy);
    }

    private void TankDestroy(ID_Settings_CS tankId, ID_Settings_CS bulletInitiatorID)
    {
        TurretDestroyProcess();
        StartCoroutine(BodyExplosionSeqence());
        CreateNavMeshObstacle();
        StartCoroutine(Disable_MainBody_Constraints());
    }

    private void TurretDestroyProcess()
    {
        // Create the destroyed effect.

        Instantiate(_turretProps.DestroyedEffect, _turretProps.Turret.transform.position, Quaternion.identity, _turretProps.Turret.TurretMesh);

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
    }

    private IEnumerator BodyExplosionSeqence()
    {
        ParticleSystem Destroyed_Effect = _bodyExplosionProps.Destroyed_Effect;
        Vector3 Destroyed_Effect_Offset = _bodyExplosionProps.Destroyed_Effect_Offset;
        //Vector3 pos = transform.position + (transform.right * Destroyed_Effect_Offset.x) + (transform.up * Destroyed_Effect_Offset.y) + (transform.forward * Destroyed_Effect_Offset.z);
        Instantiate(Destroyed_Effect, transform.position, transform.rotation, transform);

        yield return new WaitForSeconds(Destroyed_Effect.main.duration * 0.5f);
        ParticleSystem smoke_Effect = _bodyExplosionProps.SmokeEffectPrefab;
        Instantiate(smoke_Effect, transform.position, transform.rotation, transform);
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
