using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{

    [RequireComponent(typeof(Canvas))]
    public class HitPointsUIPresenter : UIPresenterBase
    {
        /*
		 * This script is attached to the "Canvas_HP_Bars (Self)" in the scene.
		 * This script controls the Hit Points Bars for displaying the current selected tank's hit points values.
		 * This script works in combination with "Damage_Control_Center_CS" in the current selected tank.
		*/


        // User options >>
        public Image Body_Bar;
        public Image Turret_Bar;
        public Image Left_Track_Bar;
        public Image Right_Track_Bar;
        public float Flash_Time = 1.0f;
        // << User options
        Color initialColor;
        //float previousRightTrackHP;
        int flashCancelID;
        //DamageReceiversManager damageScript;
        //Image[] bodyBarImages;
        //Image[] turretBarImages;


        

        //Image[] leftTrackBarImages;
        //Image[] rightTrackBarImages;
        //float previousBodyHP;
        //float previousTurretHP;
        //float previousLeftTrackHP;

        internal void BodyDamageShow(float currentHP, float maxHP)
        {
            Body_Bar.fillAmount = currentHP / maxHP;
        }

        internal void TurretDamageShow(float currentHP, float maxHP)
        {
            Turret_Bar.fillAmount = currentHP / maxHP;
        }
        public void TrackDamageShow(TrackDamageRecivier track)
        {
            if (!track.IsRightSide)
                Left_Track_Bar.fillAmount = track.CurrentHP / track.MaxHP;
            else
                Right_Track_Bar.fillAmount = track.CurrentHP / track.MaxHP;
        }

        internal void TrackRestored(TrackDamageRecivier track)
        {
            if (!track.IsRightSide)
                Left_Track_Bar.fillAmount = 1;
            else
                Right_Track_Bar.fillAmount = 1;
        }

        internal void TrackBreached(TrackDamageRecivier track)
        {
            StartCoroutine(RestoreTrackAnimation(track));
        }

        private IEnumerator RestoreTrackAnimation(TrackDamageRecivier track)
        {
            Image currentTrackImage = track.IsRightSide ? Right_Track_Bar : Left_Track_Bar;
            float timer = 0;
            float duration = track.RepairDuration;
            while (currentTrackImage.fillAmount < 1)
            {
                timer += Time.deltaTime;
                float alpha = timer / duration;
                currentTrackImage.fillAmount = alpha;

                yield return null;
            }
        }

        public override void InitialCanvas()
        {
            base.InitialCanvas();
            // Store the initial color.
            initialColor = Body_Bar.color;

            // Get all the child images.
            //bodyBarImages = Body_Bar.GetComponentsInChildren<Image>();
            //turretBarImages = Turret_Bar.GetComponentsInChildren<Image>();
            //leftTrackBarImages = Left_Track_Bar.GetComponentsInChildren<Image>();
            //rightTrackBarImages = Right_Track_Bar.GetComponentsInChildren<Image>();

            Left_Track_Bar.fillAmount = 1;
            Right_Track_Bar.fillAmount = 1;
            Body_Bar.fillAmount = 1;
            Turret_Bar.fillAmount = 1;
        }

        private IEnumerator Flash(Image[] images, int id)
        {
            flashCancelID = 0;
            float count = 0.0f;
            while (count < Flash_Time)
            {
                if (id == flashCancelID)
                {
                    yield break;
                }
                Color currentColor = initialColor;
                for (int i = 0; i < images.Length; i++)
                {
                    currentColor.a = Mathf.Lerp(1.0f, initialColor.a, count / Flash_Time);
                    images[i].color = currentColor;
                }
                count += Time.deltaTime;
                yield return null;
            }
        }
    }

}