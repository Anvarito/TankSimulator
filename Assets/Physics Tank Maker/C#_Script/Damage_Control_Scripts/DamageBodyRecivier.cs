using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

    public class DamageBodyRecivier : DamageRecivierBase
    {
        void Update()
        {
            // Check the rollover.
            if (Mathf.Abs(Mathf.DeltaAngle(0.0f, transform.eulerAngles.z)) > 90.0f)
            {
                //centerScript.Receive_Damage(Mathf.Infinity, 0, 0); // type = 0 (MainBody), index =0 (useless)
                ParthDestroy(null);
            }
        }

        protected override void ParthDestroy(ID_Settings_CS bulletLauncherID)
        {
            base.ParthDestroy(bulletLauncherID);
            transform.root.tag = "Finish";

            MainBody_Destroyed_Linkage();
        }
        

        void MainBody_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS", when the MainBody has been destroyed.
            Destroy(this);
        }

    }

}
