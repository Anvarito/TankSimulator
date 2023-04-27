using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

    public class DamageStatickTrackCollider : DamageBase
    {

        public Static_Track_Piece_CS Linked_Piece_Script; //Linked in editor version of script

        // For editor script.
        [Header("Collider damage")]
        [SerializeField] private float _InitialTrackHP = 1000.0f;

        [Header("Repairing")]
        [SerializeField] private bool _repairableTrack = true;
        [SerializeField] private float _repairDuration = 20.0f;

        private float Track_Repairing_Velocity_Limit = 0.5f;
        private IEnumerator _repairRoutine;

        void Awake()
        { // (Note.) The hierarchy must be changed before "Start()", because this gameobject might be inactivated by the "Track_LOD_Control_CS" in the "Start()".

            // Change the hierarchy. (Make this Track_Collider a child of the MainBody.)
            transform.parent = transform.parent.parent;
        }

        public override void Initialize(RecivierSettings recivierSettings)
        {
            base.Initialize(recivierSettings);
            gameObject.layer = Layer_Settings_CS.Armor_Collider_Layer;
            // Make this invisible.
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                meshRenderer.enabled = false;
            }
        }
        //[ContextMenu("DADA")]
        //public void DADA()
        //{
        //    Get_Damage(60, 1);
        //}

        protected override void ParthDestroy()
        {
            base.ParthDestroy();
            if (_repairableTrack)
            {
                // Check the tank has "Static_Track" or "Scroll_Track".
                GetComponent<Collider>().enabled = false;
            }

            if (Linked_Piece_Script)
            {
                Linked_Piece_Script.Start_Breaking(_repairDuration);
            }

        }

        //private void TrackDestroyed()
        //{
        //    // Send message to "Damage_Control_02_Physics_Track_Piece_CS", "Fix_Shaking_Rotation_CS", "Damage_Control_04_Track_Collider_CS", "Static_Track_Piece_CS", "Track_Joint_CS", "Stabilizer_CS", "Drive_Wheel_CS", "Static_Wheel_CS", "Track_Scroll_CS", "Track_LOD_Control_CS", "Static_Track_Switch_Mesh_CS".
        //    //bodyTransform.BroadcastMessage("Track_Destroyed_Linkage", isLeft, SendMessageOptions.DontRequireReceiver);

        //    // Start repairing the tracks.

        //}

        public void StartRepairing()
        {
            if (_repairRoutine == null)
                StartCoroutine(Track_Repairing_Timer());
        }

        private IEnumerator Track_Repairing_Timer()
        {
            var repairingTimer = 0.0f;
            while (repairingTimer < _repairDuration)
            {
                repairingTimer += Time.deltaTime;

                // Set the HP.
                _hitPoints = _InitialTrackHP * (repairingTimer / _repairDuration);

                yield return null;
            }

            //// Check the tank is still alive.
            //if (isDead)
            //{
            //    yield break;
            //}

            // Repair the tracks.
            Track_Repaired_Linkage();
        }


        //void Track_Repaired(bool isLeft)
        //{
        //    // Send message to "Damage_Control_04_Track_Collider_CS", "Static_Track_Piece_CS", "Stabilizer_CS", "Drive_Wheel_CS", "Static_Wheel_CS", "Track_Scroll_CS", "Track_LOD_Control_CS", "Static_Track_Switch_Mesh_CS".
        //    bodyTransform.BroadcastMessage("Track_Repaired_Linkage", isLeft, SendMessageOptions.DontRequireReceiver);
        //}

        void Track_Repaired_Linkage()
        { // Called from "Damage_Control_Center_CS".
            // Enable the collider.
            GetComponent<Collider>().enabled = true;
        }

    }
}


