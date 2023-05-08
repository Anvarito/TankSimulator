using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

	public abstract class DamageRecivierBase : MonoBehaviour, IDamageble
	{
		public float CurentHP { get; internal set; }
		public float MaxHP { get; internal set; }

		protected float _damageThreshold;

		[HideInInspector] public UnityEvent OnDestroyed;
		[HideInInspector] public UnityEvent<float, float> OnDamaged;

		public float DamageTreshold => _damageThreshold;

        public virtual void Initialize(RecivierSettings recivierSettings)
        {
			CurentHP = recivierSettings.MaxHitPoints;
			_damageThreshold = recivierSettings.DamageTreshold;
			   MaxHP = CurentHP;
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
			CurentHP -= damage;
			OnDamaged?.Invoke(CurentHP, MaxHP);
			if (CurentHP <= 0)
			{
				ParthDestroy();
			}
		}

		protected virtual void ParthDestroy() 
		{
			CurentHP = 0;
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
