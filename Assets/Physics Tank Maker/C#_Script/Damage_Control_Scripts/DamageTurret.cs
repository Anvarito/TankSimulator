namespace ChobiAssets.PTM
{
    
    public class DamageTurret : DamageRecivierBase
    {
        public override void Initialize(RecivierSettings recivierSettings)
        {
            if(_additionsDamage.Count == 0)
            _additionsDamage.AddRange(transform.GetComponentsInChildren<AdditionalDamageZone>());

            base.Initialize(recivierSettings);
        }
        protected override void ParthDestroy()
        {
            base.ParthDestroy();
            Turret_Destroyed_Linkage();
        }

        void Turret_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS", when this turret or the parent turret has been destroyed.
            //Destroy(this);
        }
    }
}
