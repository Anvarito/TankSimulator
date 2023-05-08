using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ChobiAssets.PTM
{

    public class DamageBodyRecivier : DamageRecivierBase
    {

        //[SerializeField] private float MainBody_Damage_Threshold = 100.0f;
        
        [SerializeField] private List<DamageAdditionalZone> _additionsDamage;

        public override void Initialize(RecivierSettings recivierSettings)
        {
            base.Initialize(recivierSettings);
            if (_additionsDamage.Count != 0)
            {
                foreach (var armor in _additionsDamage)
                {
                    if (armor == null) continue;
                    armor.Initialize(_damageThreshold);
                    armor.OnArmorDamage.AddListener(AdditionalZoneDamaged);
                }
            }
            else
            {
                foreach (Transform armor in transform)
                {
                    if (armor.TryGetComponent(out DamageAdditionalZone damageReciver))
                        _additionsDamage.Add(damageReciver);
                }
            }
        }

        private void AdditionalZoneDamaged(float damage, int bulletType)
        {
            DealDamage(damage, bulletType);
        }

        

        void Update()
        {
            // Check the rollover.
            if (Mathf.Abs(Mathf.DeltaAngle(0.0f, transform.eulerAngles.z)) > 90.0f)
            {
                //centerScript.Receive_Damage(Mathf.Infinity, 0, 0); // type = 0 (MainBody), index =0 (useless)
                ParthDestroy();
            }
        }

        protected override void ParthDestroy()
        {
            base.ParthDestroy();
            // Set the tag.
            transform.root.tag = "Finish";

            MainBody_Destroyed_Linkage();
        }




        void MainBody_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS", when the MainBody has been destroyed.
            Destroy(this);
        }

    }

}
