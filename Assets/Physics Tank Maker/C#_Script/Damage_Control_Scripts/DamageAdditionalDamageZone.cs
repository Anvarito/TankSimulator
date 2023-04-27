using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{
    [RequireComponent(typeof(MeshRenderer))]
    public class DamageAdditionalDamageZone : MonoBehaviour, IDamageble
    {

        [SerializeField] private float Damage_Multiplier = 1.0f;
        [HideInInspector] public UnityEvent<float, int> OnArmorDamage;

        private float _damageThreshold;

        //DamageBase parentDamageScript;
        private void Start()
        {
            // Make this invisible.
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer)
            {
                meshRenderer.enabled = false;
            }
        }
        public void Initialize(float treshold)
        {
            gameObject.layer = Layer_Settings_CS.Armor_Collider_Layer;
            _damageThreshold = treshold;
            // Find the "Damage_Control_##_##_CS" script in the parent object.
            //parentDamageScript = transform.parent.GetComponent<DamageBase>();
            //if (parentDamageScript == null)
            //{
            //    Destroy(this.gameObject);
            //}
        }

        public void DealDamage(float damage, int bulletType)
        { // Called from "Bullet_Control_CS".

            // Apply the multiplier.
            if (bulletType == 0)
            { // AP
                damage *= Damage_Multiplier;
            }

            OnArmorDamage?.Invoke(damage, bulletType);
        }

        public bool CheckBreackout(float damage, int bulletType)
        {
            if (damage < _damageThreshold)
            { // Never receive any damage under the threshold value.
                return false;
            }

            return true;
        }


        void MainBody_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS", when the MainBody has been destroyed.
            Destroy(this.gameObject);
        }


        void Turret_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS", when this turret or the parent turret has been destroyed.
            Destroy(this.gameObject);
        }

    }

}
