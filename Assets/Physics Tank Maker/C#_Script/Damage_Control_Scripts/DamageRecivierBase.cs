using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

    public abstract class DamageRecivierBase : MonoBehaviour, IDamageble
    {
        [SerializeField] protected List<AdditionalDamageZone> _additionsDamage;
        public float CurentHP { get; internal set; }
        public float MaxHP { get; internal set; }

        protected float _damageThreshold;

        [HideInInspector] public UnityEvent<ID_Settings_CS> OnDestroyed;
        [HideInInspector] public UnityEvent<float, float> OnDamaged;

        public float DamageTreshold => _damageThreshold;

        public virtual void Initialize(RecivierSettings recivierSettings)
        {
            CurentHP = recivierSettings.MaxHitPoints;
            _damageThreshold = recivierSettings.DamageTreshold;
            MaxHP = CurentHP;

            if (_additionsDamage.Count != 0)
            {
                Subscribe();
            }
        }

        private void Subscribe()
        {
            foreach (var armor in _additionsDamage)
            {
                if (armor == null) continue;
                armor.Initialize(_damageThreshold);
                armor.OnArmorDamage.AddListener(AdditionalZoneDamaged);
            }
        }

        private void Unsubscribe()
        {
            foreach (var armor in _additionsDamage)
            {
                if (armor == null) continue;
                armor.Initialize(_damageThreshold);
                armor.OnArmorDamage.RemoveListener(AdditionalZoneDamaged);
            }
        }

        private void AdditionalZoneDamaged(float damage, ID_Settings_CS bulletLauncherID)
        {
            DealDamage(damage, bulletLauncherID);
        }

        [ContextMenu("DamageTest")]
        public void DADA()
        {
            DealDamage(100000000, null);
        }
        public virtual void DealDamage(float damage, ID_Settings_CS bulletLauncherID)
        {
            CurentHP -= damage;
            OnDamaged?.Invoke(CurentHP, MaxHP);
            //print(damage);
            if (CurentHP <= 0)
            {
                ParthDestroy(bulletLauncherID);
            }
        }

        protected virtual void ParthDestroy(ID_Settings_CS bulletLauncherID)
        {
            CurentHP = 0;
            Unsubscribe();
            OnDestroyed?.Invoke(bulletLauncherID);
        }

        public virtual bool CheckBreackout(float damage)
        {
            if (CurentHP <= 0)
                return false;

            if (damage < _damageThreshold)
            { // Never receive any damage under the threshold value.
                return false;
            }

            return true;
        }

    }

}
