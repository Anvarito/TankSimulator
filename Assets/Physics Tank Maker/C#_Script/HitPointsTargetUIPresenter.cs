using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ChobiAssets.PTM
{

    [DefaultExecutionOrder(+2)] // (Note.) This script is executed after the main camera is moved, in order to move the bars smoothly.
    [RequireComponent(typeof(Canvas))]
    public class HitPointsTargetUIPresenter : UIPresenterBase
    {
        /*
		 * This script is attached to the "Canvas_HP_Bars (Target)" in the scene.
		 * This script controls the Hit Points Bars for displaying the target tank's hit points values.
		 * This script works in combination with "Aiming_Control_CS" in the current selected tank, and "Damage_Control_Center_CS" in the target tank.
		*/


        // User options >>
        public Canvas This_Canvas;
        public CanvasScaler This_Canvas_Scaler;
        public Transform Bars_Parent_Transform;
        public Image Body_Bar;
        public Image Turret_Bar;
        public Image Left_Track_Bar;
        public Image Right_Track_Bar;
        public float _upperOffset = 1.0f;
        public float Flash_Time = 1.0f;
        public float Normal_Alpha = 0.4f;
        public Color Friend_Color = Color.blue;
        public Color Hostile_Color = Color.red;
        // << User options

        bool cancelDisableTimer;

        private Transform _targetTansform;
        private bool _isShow;

        //      Aiming_Control_CS aimingScript;
        //Image[] bodyBarImages;
        //Image[] turretBarImages;
        //Image[] leftTrackBarImages;
        //Image[] rightTrackBarImages;
        //float previousBodyHP;
        //float previousTurretHP;
        //float previousLeftTrackHP;
        //float previousRightTrackHP;
        //int flashCancelID;

        public void Initialize()
        {
            This_Canvas_Scaler = _canvas.GetComponent<CanvasScaler>();
            Turret_Bar.fillAmount = 1;
            Body_Bar.fillAmount = 1;
            Left_Track_Bar.fillAmount = 1;
            Right_Track_Bar.fillAmount = 1;
        }

        internal void Hide()
        {
            _targetTansform = null;
            Bars_Parent_Transform.gameObject.SetActive(false);
            _isShow = false;
        }

        internal void Show(Transform targetTransform, bool isAlly)
        {
            _targetTansform = targetTransform;
            Bars_Parent_Transform.gameObject.SetActive(true);
            _isShow = true;
            //Set_Color();
        }

        internal void Refresh(float alphaBodyHP, float alphaTurretHP, float alphaRTrackHP, float alphaLTrackHP)
        {
            Turret_Bar.fillAmount = alphaTurretHP;
            Body_Bar.fillAmount = alphaBodyHP;
            Left_Track_Bar.fillAmount = alphaLTrackHP;
            Right_Track_Bar.fillAmount = alphaRTrackHP;
        }

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
        //void LateUpdate()
        //{
        //    if (aimingScript == null)
        //    { // The tank has been destroyed.
        //        Disable_Canvas(true);
        //        return;
        //    }

        //    if (aimingScript.Is_Selected == false)
        //    { // The tank is not selected now.
        //        Disable_Canvas(true);
        //        return;
        //    }

        //    if (aimingScript.Target_Rigidbody && aimingScript.Target_Rigidbody != currentTargetRigidbody)
        //    { // The target has been changed.
        //        // Update the target information.
        //        Update_Traget_Information();
        //        return;
        //    }

        //    if (currentTargetRigidbody == null)
        //    { // The target has been removed from the scene.
        //        Disable_Canvas(true);
        //        return;
        //    }

        //    if (targetDamageScript == null)
        //    { // The target has no "Damage_Control_Center_CS".
        //        Disable_Canvas(true);
        //        return;
        //    }
        //    /*
        //    if (targetDamageScript.MainBody_HP <= 0.0f)
        //    { // The target has been destroyed.
        //        Disable_Canvas(true);
        //        return;
        //    }*/


        //    if (aimingScript.Target_Rigidbody == null)
        //    { // The tank is not locking on anything.
        //        // Disable the canvas after a while.
        //        Disable_Canvas(false);
        //    }

        //    // Set the appearance and the position of all the bars.
        //    Set_Appearance_And_Position();

        //    // Control the appearance of each bar.
        //    Control_Bars();
        //}


        void Enable_Canvas()
        {
            // Cancel the "Disable_Timer".
            cancelDisableTimer = true;

            if (This_Canvas.enabled)
            {
                return;
            }

            // Enable the canvas.
            This_Canvas.enabled = true;
        }


        void Disable_Canvas(bool isImmediate)
        {
            if (This_Canvas.enabled == false)
            {
                return;
            }

            // Disable immediately.
            if (isImmediate)
            {
                This_Canvas.enabled = false;
                return;
            }

            // Disable the canvas after a while.
            StartCoroutine("Disable_Timer");
        }



        IEnumerator Disable_Timer()
        {
            var count = 0.0f;
            while (count < 2.0f)
            {
                if (This_Canvas.enabled == false)
                { // The canvas has been disabled.
                    yield break;
                }

                if (cancelDisableTimer)
                { // The timer has been canceled.
                    cancelDisableTimer = false;
                    yield break;
                }

                count += Time.deltaTime;
                yield return null;
            }

            This_Canvas.enabled = false;
        }


        void Set_Color(Image[] images, Color color)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = color;
            }
        }

        private void Update()
        {
            if (_isShow)
            {
                //Set_Appearance_And_Position();
            }
        }

        void Set_Appearance_And_Position()
        {
            var mainCamera = _canvas.worldCamera;

            // Set the appearance and the position.
            Vector3 currentPosition = mainCamera.WorldToScreenPoint(_targetTansform.position);
            //if (currentPosition.z < 0.0f)
            //{ // Behind of the camera.
            //    // Disable the canvas.
            //    Disable_Canvas(true);
            //    return;
            //} // In front of the camera.

            //// Enable the canvas.
            //if (_targetTansform)
            //{
            //    Enable_Canvas();
            //}

            // Set the scale.
            float frustumHeight = 2.0f * Vector3.Distance(mainCamera.transform.position, _targetTansform.position) * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            Bars_Parent_Transform.localScale = Vector3.one * Mathf.Lerp(0.5f, 1.5f, 1.0f / frustumHeight);

            // Set the position.
            float resolutionOffset = Screen.width / This_Canvas_Scaler.referenceResolution.x;
            currentPosition.y += Mathf.Lerp(64.0f, 640.0f, Mathf.Sqrt(1.0f / frustumHeight)) * resolutionOffset;
            currentPosition.z = 128.0f;
            Bars_Parent_Transform.position = currentPosition;
        }


        void Control_Bars()
        {
            /* // MainBody
             Body_Bar.fillAmount = targetDamageScript.MainBody_HP / targetDamageScript.Initial_Body_HP;
             if (previousBodyHP != targetDamageScript.MainBody_HP)
             {
                 flashCancelID = 1;
                 StartCoroutine(Flash(bodyBarImages, 1));
             }

             // Turret
             Turret_Bar.fillAmount = targetDamageScript.Turret_Props[0].hitPoints / targetDamageScript.Initial_Turret_HP;
             if (previousTurretHP != targetDamageScript.Turret_Props[0].hitPoints)
             {
                 flashCancelID = 2;
                 StartCoroutine(Flash(turretBarImages, 2));
             }

             // Left Track
             Left_Track_Bar.fillAmount = targetDamageScript.Left_Track_HP / targetDamageScript.Initial_Left_Track_HP;
             if (previousLeftTrackHP != targetDamageScript.Left_Track_HP)
             {
                 flashCancelID = 3;
                 StartCoroutine(Flash(leftTrackBarImages, 3));
             }

             // Right Track
             Right_Track_Bar.fillAmount = targetDamageScript.Right_Track_HP / targetDamageScript.Initial_Right_Track_HP;
             if (previousRightTrackHP != targetDamageScript.Right_Track_HP)
             {
                 flashCancelID = 4;
                 StartCoroutine(Flash(rightTrackBarImages, 4));
             }

             // Store the HP values.
             previousBodyHP = targetDamageScript.MainBody_HP;
             previousTurretHP = targetDamageScript.Turret_Props[0].hitPoints;
             previousLeftTrackHP = targetDamageScript.Left_Track_HP;
             previousRightTrackHP = targetDamageScript.Right_Track_HP;*/
        }
    }
}