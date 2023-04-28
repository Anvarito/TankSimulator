using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

    public class TrackPieceDamage : MonoBehaviour, IDamageble
    {

        public Static_Track_Piece_CS Linked_Piece_Script; //Linked in editor version of script

        private bool _isRightSIde = false;
        private float _damageThreshold;
        private float _repairDuration;
        public bool IsRightSide => _isRightSIde;

        [HideInInspector] public UnityEvent<float, TrackPieceDamage> OnDamaged;

        void Awake()
        { // (Note.) The hierarchy must be changed before "Start()", because this gameobject might be inactivated by the "Track_LOD_Control_CS" in the "Start()".
            // Change the hierarchy. (Make this Track_Collider a child of the MainBody.)
            transform.parent = transform.parent.parent;
        }
        public void Initialize(TrackRecivierSettings recivierSettings)
        {
            _damageThreshold = recivierSettings.DamageTreshold;
            _repairDuration = recivierSettings.RepairingDuration;
            gameObject.layer = Layer_Settings_CS.Armor_Collider_Layer;
            // Make this invisible.
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                meshRenderer.enabled = false;
            }

            SetSide();
        }
        private void SetSide()
        {
            if (transform.localPosition.y > 0)
                _isRightSIde = false;
            else
                _isRightSIde = true;
        }

        public void DealDamage(float damage, int bulletType)
        {
            OnDamaged?.Invoke(damage, this);
        }

        public bool CheckBreackout(float damage, int bulletType)
        {
            if (damage < _damageThreshold)
            { // Never receive any damage under the threshold value.
                return false;
            }

            return true;
        }

        [ContextMenu("DADA")]
        public void DADA()
        {
            DealDamage(600000, 1);
        }

        public void ParthDestroy()
        {
            GetComponent<Collider>().enabled = false;

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


