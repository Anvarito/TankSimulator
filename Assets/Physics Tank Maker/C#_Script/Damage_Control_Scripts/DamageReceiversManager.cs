using System.Collections;
using Infrastructure;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

    //[ System.Serializable]
    //public struct TurretDamageControlProp
    //{
    //	public Transform turretBaseTransform;
    //	public float hitPoints;
    //	public float damageThreshold;
    //	public bool blowOff;
    //	public float mass;
    //	public GameObject destroyedEffect;
    //}


    public class DamageReceiversManager : MonoBehaviour
    {
        /* 
		 * This script is attached to the "MainBody" in the tank.
		 * This script controls the hit points of the tank parts, and their destruction processes.
		 * This script works in combination with "Damage_Control_##_##_CS" scripts in the tank parts.
		*/

        [Space(10)]
        [SerializeField] private DamageReciviersSettings _damageReciviersSettings;

        [Space(10)]
        [SerializeField] private DamageTurret _turretDamages;
        [SerializeField] private DamageBodyRecivier _mainBodyDamages;
        [SerializeField] private TracksHolder _damageTrackRecivier;

        AI_CS aiScript;

        private bool isDead;

        [HideInInspector] public UnityEvent<float, float> OnTurretDamaged;
        [HideInInspector] public UnityEvent<float, float> OnBodyDamaged;

        [HideInInspector] public UnityEvent<TrackDamageRecivier> OnTrackDamaged;
        [HideInInspector] public UnityEvent<TrackDamageRecivier> OnTrackBreach;
        [HideInInspector] public UnityEvent<TrackDamageRecivier> OnTrackRestore;

        [HideInInspector] public UnityEvent<ID_Settings_CS,ID_Settings_CS> OnTankDestroyed; //id at kill initiator

        public DamageTurret TurretDamageRecivier => _turretDamages;
        public DamageBodyRecivier BodyDamageRecivier => _mainBodyDamages;
        public TracksHolder TrackDamageRecivier => _damageTrackRecivier;

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy() => 
            OnTankDestroyed.RemoveAllListeners();

        void Initialize()
        {
            InitializingAllDamageReciviers();
            SubscribeResiviers();

            OnTankDestroyed.AddListener(DestroyWithDelay);
        }
        private void InitializingAllDamageReciviers()
        {
            if (_turretDamages) _turretDamages.Initialize(_damageReciviersSettings.GetTurretsettings());
            else Debug.LogError("TurretDamages not assigned!!!");
            if (_mainBodyDamages) _mainBodyDamages.Initialize(_damageReciviersSettings.GetBodysettings());
            else Debug.LogError("MainBodyDamages not assigned!!!");
            if (_damageTrackRecivier) _damageTrackRecivier.Inititalize(_damageReciviersSettings.GetTracksettings());
            else Debug.LogError("DamageTrackRecivier not assigned!!!");
        }
        private void SubscribeResiviers()
        {
            if (_turretDamages)
            {
                _turretDamages.OnDamaged.AddListener(TurretDamage);
                _turretDamages.OnDestroyed.AddListener(TurretDestroy);
            }
            else Debug.LogError("TurretDamages not assigned!!!");

            if (_mainBodyDamages)
            {
                _mainBodyDamages.OnDamaged.AddListener(BodyDamage);
                _mainBodyDamages.OnDestroyed.AddListener(BodyDestroy);

            }
            else Debug.LogError("MainBodyDamages not assigned!!!");

            if (_damageTrackRecivier)
            {
                _damageTrackRecivier.OnTrackDamaged.AddListener(TrackDamaged);
                _damageTrackRecivier.OnTrackDestroyed.AddListener(TrackDestroyed);
                _damageTrackRecivier.OnTrackRestored.AddListener(TrackRestored);
            }
            else Debug.LogError("DamageTrackRecivier not assigned!!!");
        }

        private void BodyDamage(float currentHP, float maxHP)
        {
            OnBodyDamaged?.Invoke(currentHP, maxHP);
        }
        private void TurretDamage(float currentHP, float maxHP)
        {
            OnTurretDamaged?.Invoke(currentHP, maxHP);
        }


        private void TrackDamaged(TrackDamageRecivier track)
        {
            OnTrackDamaged?.Invoke(track);
        }

        private void TrackRestored(TrackDamageRecivier track)
        {
            OnTrackRestore?.Invoke(track);
        }

        private void TrackDestroyed(TrackDamageRecivier track)
        {
            OnTrackBreach?.Invoke(track);
        }


        private void BodyDestroy(ID_Settings_CS bulletInitiatorID)
        {
            TankDestroyed(bulletInitiatorID);
        }

        private void TurretDestroy(ID_Settings_CS bulletInitiatorID)
        {
            TankDestroyed(bulletInitiatorID);
        }

        private void TankDestroyed(ID_Settings_CS bulletInitiatorID)
        {
            isDead = true;
            if (_damageTrackRecivier) _damageTrackRecivier.FullBreak();

            _turretDamages.OnDestroyed.RemoveListener(TurretDestroy);
            _mainBodyDamages.OnDestroyed.RemoveListener(BodyDestroy);
            _turretDamages.OnDamaged.RemoveListener(TurretDamage);
            _mainBodyDamages.OnDamaged.RemoveListener(BodyDamage);

            OnTankDestroyed?.Invoke(GetComponentInParent<ID_Settings_CS>(),bulletInitiatorID);
        }

        private void DestroyWithDelay(ID_Settings_CS victim, ID_Settings_CS killer)
        {
            OnTankDestroyed.RemoveListener(DestroyWithDelay);

            StartCoroutine(DestroyCoroutine(Constants.DestroyDelay));
        }

        private IEnumerator DestroyCoroutine(float destroyDelay)
        {
            var endTime = Time.time + destroyDelay;
            while (endTime > Time.time)
                yield return null;
            
            Destroy(transform.parent.gameObject);
            
        }


        public bool Receive_Damage(float damage, int type, int index)
        { // Called from "Damage_Control_##_##_CS" scripts in the tank.
            if (aiScript)
            { // AI tank.
                // Call "AI_CS" to disable the dead angle.
                aiScript.StartCoroutine("Wake_Up_Timer");
            }

            return false;
        }
        void Selected(bool isSelected)
        { // Called from "ID_Settings_CS".
            if (isSelected == false)
            {
                return;
            } // This tank is selected.

            //// Send this reference to the "UI_HP_Bars_Self_CS" in the scene.
            //if (UI_HP_Bars_Self_CS.Instance)
            //{
            //    UI_HP_Bars_Self_CS.Instance.Get_Damage_Script(this);
            //}
        }


        void Get_AI_CS(AI_CS aiScript)
        { // Called from "AI_CS".
            this.aiScript = aiScript;
        }

    }

}