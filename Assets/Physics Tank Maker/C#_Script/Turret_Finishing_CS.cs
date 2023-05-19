using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

    public class Turret_Finishing_CS : MonoBehaviour
    {
        /*
		 * This script is attached to the "Turret Objects" in the tank.
		 * This script change the hierarchy of the child objects such as "Turret_Base", "Cannon_Base" and "Barrel_Base" at the first time.
		*/


        // User options >>
        public bool Child_Flag = false;
        public Transform Parent_Transform;
        // << User options


        Transform thisTransform;
        Transform turretBase;
        Transform cannonBase;
        Transform barrelBase;


        void Single_Barrel()
        {
            turretBase = thisTransform.Find("Turret_Base");
            cannonBase = thisTransform.Find("Cannon_Base");
            barrelBase = thisTransform.Find("Barrel_Base");
            if (turretBase && cannonBase && barrelBase)
            {
                // Change the hierarchy.
                barrelBase.parent = cannonBase;
                cannonBase.parent = turretBase;
                Finishing();
            }
            else
            {
                Error_Message();
            }
        }




        void Finishing()
        {
            if (Child_Flag == false)
            { // The turret is not a child turret.
                Destroy(this);
            }
        }

        public void Assembling()
        {
            thisTransform = transform;
            Single_Barrel();

            //// Change the hierarchy of the child turret.
            //if (Parent_Transform)
            //{
            //    thisTransform.parent = Parent_Transform;
            //}
            //else
            //{
            //    Debug.LogError("'Parent_Transform' for the child Turret is not assigned.");
            //}

            Destroy(this);
        }
      
        void Error_Message()
        {
            Debug.LogError("'Turret_Finishing_CS' could not change the hierarchy of the turret.");
            Debug.LogWarning("Make sure the names of 'Turret_Base', 'Cannon_Base' and 'Barrel_Base'.");
            Destroy(this);
        }

    }

}