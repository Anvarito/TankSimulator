using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{
    [RequireComponent(typeof(Rigidbody))]
    public class Drive_Wheel_CS : MonoBehaviour
    {
        /*
		 * This script is attached to all the drive wheels in the tank.
		 * This script controls the angular velocity and angular drug of the rigidbody in the wheel, and give it the torque force.
		 * This script works in combination with "Drive_Control_CS" in the MainBody, and "Drive_Wheel_Parent_CS" in the parent object.
		*/

        // User options >>
        [SerializeField] private bool Is_Left;
        private Rigidbody This_Rigidbody;
        //public Drive_Wheel_Parent_CS Parent_Script;
        // << User options
        private void Awake()
        {
            This_Rigidbody = GetComponent<Rigidbody>();
        }
        public void SetSide(bool value)
        {
            Is_Left = value;
        }

        public void SetRigidbody(Rigidbody rb)
        {
            This_Rigidbody = rb;
        }

        float storedSphereColliderRadius;

        public Rigidbody GetRigidbody()
        {
            return This_Rigidbody;
        }

        public bool IsLeft()
        {
            return Is_Left;
        }


        void MainBody_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS".

            // Lock the wheel.
            This_Rigidbody.angularDrag = Mathf.Infinity;

            // Disable this script. (Note.) Don't remove this script, because if the tracks are still alive, the "Track_Destroyed_Linkage()" might been called.
            this.enabled = false;
        }


        public void Track_Destroyed_Linkage(bool isLeft)
        { // Called from "Damage_Control_Center_CS".
            if (isLeft != Is_Left)
            { // The direction is different.
                return;
            }

            // Lock the wheels.
            This_Rigidbody.angularDrag = Mathf.Infinity;

            // Store the radius of the SphereCollider.
            var sphereCollider = GetComponent<SphereCollider>();
            storedSphereColliderRadius = sphereCollider.radius;

            // Resize the SphereCollider.
            var meshFilter = GetComponent<MeshFilter>();
            if (meshFilter && meshFilter.sharedMesh)
            {
                sphereCollider.radius = meshFilter.sharedMesh.bounds.extents.x;
            }

            // Disable this script.
            //this.enabled = false;
        }


        public void Track_Repaired_Linkage(bool isLeft)
        { // Called from "Damage_Control_Center_CS".
            if (isLeft != Is_Left)
            { // The direction is different.
                return;
            }

            // Resize the SphereCollider.
            var sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = storedSphereColliderRadius;

            // Enable this script.
            //this.enabled = true;
        }

    }

}