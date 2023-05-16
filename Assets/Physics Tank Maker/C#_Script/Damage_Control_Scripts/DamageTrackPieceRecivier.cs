using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{
    [RequireComponent(typeof(Collider))]
    public class DamageTrackPieceRecivier : MonoBehaviour, IDamageble
    {

        public Static_Track_Piece_CS Linked_Piece_Script; //Linked in editor version of script
        
        public bool IsRightSide => !Linked_Piece_Script.Is_Left;

        public float DamageTreshold { get; private set; }

        [HideInInspector] public UnityEvent<float, DamageTrackPieceRecivier> OnDamaged;
        //private Collider _collider;

        void Awake()
        { // (Note.) The hierarchy must be changed before "Start()", because this gameobject might be inactivated by the "Track_LOD_Control_CS" in the "Start()".
            // Change the hierarchy. (Make this Track_Collider a child of the MainBody.)
            //transform.parent = transform.parent.parent;
            //_collider = GetComponent<Collider>();
            GetComponent<MeshRenderer>().enabled = false;
        }
        public void Initialize(RecivierSettings recivierSettings)
        {
            DamageTreshold = recivierSettings.DamageTreshold;

            gameObject.layer = Layer_Settings_CS.Armor_Collider_Layer;

            // Make this invisible.
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                meshRenderer.enabled = false;
            }
        }

        [ContextMenu("Damage")]
        private void DADA()
        {
            DealDamage(1000, 1);
        }
        public void DealDamage(float damage, int bulletType)
        {
            OnDamaged?.Invoke(damage, this);
        }

        public bool CheckBreackout(float damage, int bulletType)
        {
            if (damage < DamageTreshold)
            { // Never receive any damage under the threshold value.
                return false;
            }

            return true;
        }

        public Static_Track_Piece_CS GetSelfTrackPiece()
        {
            return Linked_Piece_Script;
        }
    }
}


