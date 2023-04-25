using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

	public class DamageMainBody : DamageBase
	{

        Transform bodyTransform;
        public float MainBody_HP = 1000.0f;
        public float MainBody_Damage_Threshold = 100.0f;
        public GameObject Destroyed_Effect;
        public Vector3 Destroyed_Effect_Offset;
        public float Initial_Body_HP;
        public UnityEvent OnBodyDestroy;
        protected override void Start()
        {
            base.Start();

            Initial_Body_HP = MainBody_HP;
            bodyTransform = transform;
        }


        void Update()
        {
            // Check the rollover.
            if (Mathf.Abs(Mathf.DeltaAngle(0.0f, bodyTransform.eulerAngles.z)) > 90.0f)
            {
                //centerScript.Receive_Damage(Mathf.Infinity, 0, 0); // type = 0 (MainBody), index =0 (useless)
                MainBodyDamaged(Mathf.Infinity);
            }
        }


        public override bool Get_Damage(float damage, int bulletType)
        { // Called from "Bullet_Control_CS", when the bullet hits this collider.
          // Send the damage value to the "Damage_Control_Center_CS".
            return MainBodyDamaged(damage);
        }

        bool MainBodyDamaged(float damage)
        {
            if (damage < MainBody_Damage_Threshold)
            { // Never receive any damage under the threshold value.
                return false;
            }

            MainBody_HP -= damage;
            if (MainBody_HP <= 0)
            {
                MainBody_Destroyed();
                return true;
            }
            return false;
        }
        void MainBody_Destroyed()
        {
            // Check the tank has already been dead or not.
            //if (isDead)
            //{ // (Note.) When the tank has been destroyed by an explosion, this function might be called several times in the one frame.
            //    return;
            //}
            //isDead = true;

            // Set the HP value to zero.
            MainBody_HP = 0.0f;

            // Set the tag.
            bodyTransform.root.tag = "Finish";

            // Destroy all the turret.
            //for (int i = 0; i < Turret_Props.Length; i++)
            //{
            //    Turret_Destroyed(i);
            //}

            // Create the destroyed effect.
            CreateDestroyedEffect();

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Send Message to "Damage_Control_00_MainBody_CS", "Damage_Control_01_Turret_CS", "Respawn_Controller_CS", "AI_CS", "UI_Aim_Marker_Control_CS", "Aiming_Marker_Control_CS", "Drive_Control_CS", "Drive_Wheel_Parent_CS", "Drive_Wheel_CS", "Steer_Wheel_CS", "Stabilizer_CS", "Fix_Shaking_Rotation_CS", "Sound_Control_##_CS".
            //bodyTransform.parent.BroadcastMessage("MainBody_Destroyed_Linkage", SendMessageOptions.DontRequireReceiver);
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
                Vector3 pos = bodyTransform.position + (bodyTransform.right * Destroyed_Effect_Offset.x) + (bodyTransform.up * Destroyed_Effect_Offset.y) + (bodyTransform.forward * Destroyed_Effect_Offset.z);
                Instantiate(Destroyed_Effect, pos, bodyTransform.rotation, bodyTransform);
            }
        }

        private void CreateNavMeshObstacle()
        {
            NavMeshObstacle navMeshObstacle = bodyTransform.gameObject.AddComponent<NavMeshObstacle>();
            navMeshObstacle.shape = NavMeshObstacleShape.Capsule;
            navMeshObstacle.carving = true;
            navMeshObstacle.carvingMoveThreshold = 1.0f;
        }

        IEnumerator Disable_MainBody_Constraints()
        {
            // Disable the rigidbody constraints in the MainBody in order to release the parking brake.
            yield return new WaitForFixedUpdate(); // This wait is required for PhysX.
            Rigidbody bodyRigidBody = bodyTransform.GetComponent<Rigidbody>();
            bodyRigidBody.constraints = RigidbodyConstraints.None;
        }



        void MainBody_Destroyed_Linkage()
		{ // Called from "Damage_Control_Center_CS", when the MainBody has been destroyed.
            OnBodyDestroy?.Invoke();
            Destroy (this);
		}

	}

}
