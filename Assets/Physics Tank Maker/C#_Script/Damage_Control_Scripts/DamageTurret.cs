using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{
    [System.Serializable]
    public struct TurretDamageControlProp
    {
        public Transform turretBaseTransform;
        public float hitPoints;
        public float damageThreshold;
        public bool blowOff;
        public float mass;
        public GameObject destroyedEffect;
    }
    public class DamageTurret : DamageBase
    {

        [SerializeField] private TurretDamageControlProp _turretProps;
        public float _initialHP;
        public UnityEvent OnTurretDestroy;
        protected override void Start()
        {
            base.Start();
            _initialHP = _turretProps.hitPoints;
        }
        public override bool Get_Damage(float damage, int bulletType)
        { // Called from "Bullet_Control_CS", when the bullet hits this collider.
          // Send the damage value to the "Damage_Control_Center_CS".
          // return centerScript.Receive_Damage(damage, 1, Turret_Index); // type = 1 (Turret), index = Turret_ID (0 = Main turret).
            return Turret_Damaged(damage);
        }

        bool Turret_Damaged(float damage)
        {
            if (_turretProps.turretBaseTransform == null)
            { // The turret had already been destroyed.
                return false;
            }

            if (damage < _turretProps.damageThreshold)
            { // Never receive any damage under the threshold value.
                return false;
            }

            _turretProps.hitPoints -= damage;
            if (_turretProps.hitPoints <= 0)
            {
                Turret_Destroyed();
                return true;
            }
            return false;
        }

        void Turret_Destroyed()
        {
            // Set the HP value to zero.
            _turretProps.hitPoints = 0.0f;

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

            // Remove the "turretBaseTransform" from the array element.
            _turretProps.turretBaseTransform = null;

            Turret_Destroyed_Linkage();
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
            // Change the hierarchy.
            //Turret_Props[index].turretBaseTransform.parent = bodyTransform.parent; // Make it a child of the top object.
        }

        void MainBody_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS", when the MainBody has been destroyed.
            Destroy(this);
        }


        void Turret_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS", when this turret or the parent turret has been destroyed.
            OnTurretDestroy?.Invoke();
            Destroy(this);
        }
    }

}
