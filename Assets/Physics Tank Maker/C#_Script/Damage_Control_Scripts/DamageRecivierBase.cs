using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

	public abstract class DamageRecivierBase : MonoBehaviour, IDamageble
	{
		protected float _hitPoints;
		protected float _initialHP;
		protected float _damageThreshold;

		[HideInInspector] public UnityEvent OnDestroyed;

		public float DamageTreshold => _damageThreshold;

        public virtual void Initialize(RecivierSettings recivierSettings)
        {
			_hitPoints = recivierSettings.MaxHitPoints;
			_damageThreshold = recivierSettings.DamageTreshold;
			   _initialHP = _hitPoints;
		}
		protected virtual void Start()
		{
		}
		[ContextMenu("DamageTest")]
		public void DADA()
		{
			DealDamage(100000000, 0);
		}
		public virtual void DealDamage(float damage, int bulletType)
        {
			_hitPoints -= damage;
			if (_hitPoints <= 0)
			{
				ParthDestroy();
			}
		}

		protected virtual void ParthDestroy() 
		{
			_hitPoints = 0;
			OnDestroyed?.Invoke();
		}

		public virtual bool CheckBreackout(float damage, int bulletType)
		{
			if (damage < _damageThreshold)
			{ // Never receive any damage under the threshold value.
				return false;
			}

			return true;
		}

	}

}
