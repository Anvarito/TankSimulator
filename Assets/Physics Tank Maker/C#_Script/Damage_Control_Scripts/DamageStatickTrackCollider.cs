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
        [SerializeField] private float _colliderDamageThreshold = 50.0f;

        [Header("Repairing")]
        [SerializeField] private bool _repairableTrack = true;
        [SerializeField] private float _trackRepairingTime = 20.0f;

        private float Track_Repairing_Velocity_Limit = 0.5f;
        private float _colliderHP;

        [HideInInspector] public UnityEvent OnTrackDestroyed;

        void Awake()
        { // (Note.) The hierarchy must be changed before "Start()", because this gameobject might be inactivated by the "Track_LOD_Control_CS" in the "Start()".

            // Change the hierarchy. (Make this Track_Collider a child of the MainBody.)
            transform.parent = transform.parent.parent;
        }


        protected override void Start()
        {
            // Set the layer.
            gameObject.layer = Layer_Settings_CS.Armor_Collider_Layer;
            _colliderHP = _InitialTrackHP;
            // Make this invisible.
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                meshRenderer.enabled = false;
            }

            base.Start();

        }

        [ContextMenu("DADA")]
        public void DADA()
        {
            Get_Damage(60, 1);
        }

        public override bool Get_Damage(float damage, int bulletType)
        { // Called from "Bullet_Control_CS", when the bullet hits this collider.

            // Send the damage value to the "Damage_Control_Center_CS".
            if (IsTrackBreach(damage))
            { // type = 3 (Track_Collider), index = Track_Index (0 = Left, 1 = Right). true = The track has been destroyed.

                // Breaking the track by calling the "Linked_Piece_Script". ("Static_Track_Piece_CS" script in the track piece.)
                if (Linked_Piece_Script)
                {
                    Linked_Piece_Script.Start_Breaking(_trackRepairingTime);
                }
                return true;
            }
            else
            { // The track has not been destroyed.
                return false;
            }
        }

        private bool IsTrackBreach(float damage)
        {
            if (damage < _colliderDamageThreshold)
            { // Never receive any damage under the threshold value.
                return false;
            }

            if (_colliderHP <= 0.0f)
            { // The track is already broken.
                return false;
            }

            _colliderHP -= damage;
            if (_colliderHP <= 0)
            {
                Track_Destroyed();
                return true;
            }
            return false;
        }


        private void Track_Destroyed()
        {
            // Send message to "Damage_Control_02_Physics_Track_Piece_CS", "Fix_Shaking_Rotation_CS", "Damage_Control_04_Track_Collider_CS", "Static_Track_Piece_CS", "Track_Joint_CS", "Stabilizer_CS", "Drive_Wheel_CS", "Static_Wheel_CS", "Track_Scroll_CS", "Track_LOD_Control_CS", "Static_Track_Switch_Mesh_CS".
            //bodyTransform.BroadcastMessage("Track_Destroyed_Linkage", isLeft, SendMessageOptions.DontRequireReceiver);

            // Start repairing the tracks.
            if (_repairableTrack)
            {
                // Check the tank has "Static_Track" or "Scroll_Track".
                GetComponent<Collider>().enabled = false;
            }

            OnTrackDestroyed?.Invoke();
        }

        public void StartRepairing()
        {
            StartCoroutine(Track_Repairing_Timer());
        }

        IEnumerator Track_Repairing_Timer()
        {
            var count = 0.0f;
            while (count < _trackRepairingTime)
            {
                count += Time.deltaTime;

                // Set the HP.
                _colliderHP = _InitialTrackHP * (count / _trackRepairingTime);

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


