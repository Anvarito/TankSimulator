namespace ChobiAssets.PTM
{
    
    public class DamageTurret : DamageRecivierBase
    {
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
