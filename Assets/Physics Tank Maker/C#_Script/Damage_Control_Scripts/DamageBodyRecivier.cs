using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

    public class DamageBodyRecivier : DamageRecivierBase
    {

        //[SerializeField] private float MainBody_Damage_Threshold = 100.0f;
        [SerializeField] private GameObject Destroyed_Effect;
        [SerializeField] private Vector3 Destroyed_Effect_Offset;
        [SerializeField] private List<DamageAdditionalZone> _additionsDamage;

        public override void Initialize(RecivierSettings recivierSettings)
        {
            base.Initialize(recivierSettings);
            if (_additionsDamage.Count != 0)
            {
                foreach (var armor in _additionsDamage)
                {
                    armor.Initialize(_damageThreshold);
                    armor.OnArmorDamage.AddListener(AdditionalZoneDamaged);
                }
            }
            else
            {
                foreach (Transform armor in transform)
                {
                    if (armor.TryGetComponent(out DamageAdditionalZone damageReciver))
                        _additionsDamage.Add(damageReciver);
                }
            }
        }

        private void AdditionalZoneDamaged(float damage, int bulletType)
        {
            DealDamage(damage, bulletType);
        }

        

        void Update()
        {
            // Check the rollover.
            if (Mathf.Abs(Mathf.DeltaAngle(0.0f, transform.eulerAngles.z)) > 90.0f)
            {
                //centerScript.Receive_Damage(Mathf.Infinity, 0, 0); // type = 0 (MainBody), index =0 (useless)
                ParthDestroy();
            }
        }

        protected override void ParthDestroy()
        {
            base.ParthDestroy();
            // Check the tank has already been dead or not.
            //if (isDead)
            //{ // (Note.) When the tank has been destroyed by an explosion, this function might be called several times in the one frame.
            //    return;
            //}
            //isDead = true;

            // Set the tag.
            transform.root.tag = "Finish";

            // Destroy all the turret.
            //for (int i = 0; i < Turret_Props.Length; i++)
            //{
            //    Turret_Destroyed(i);
            //}

            // Create the destroyed effect.
            CreateDestroyedEffect();

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Send Message to "Damage_Control_00_MainBody_CS", "Damage_Control_01_Turret_CS", "Respawn_Controller_CS", "AI_CS", "UI_Aim_Marker_Control_CS", "Aiming_Marker_Control_CS", "Drive_Control_CS", "Drive_Wheel_Parent_CS", "Drive_Wheel_CS", "Steer_Wheel_CS", "Stabilizer_CS", "Fix_Shaking_Rotation_CS", "Sound_Control_##_CS".
            //transform.parent.BroadcastMessage("MainBody_Destroyed_Linkage", SendMessageOptions.DontRequireReceiver);
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // Add NavMeshObstacle to the MainBody.
            CreateNavMeshObstacle();

            /// Release the parking brake, and Destroy this script.
            StartCoroutine(Disable_MainBody_Constraints());
            MainBody_Destroyed_Linkage();
        }

        private void CreateDestroyedEffect()
        {
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



        void MainBody_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS", when the MainBody has been destroyed.
            Destroy(this);
        }

    }

}
