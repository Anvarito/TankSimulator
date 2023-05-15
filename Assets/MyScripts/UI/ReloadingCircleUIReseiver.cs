using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace ChobiAssets.PTM
{

    public class ReloadingCircleUIReseiver : MonoBehaviour
    {
        /*
		 * This script is attached to the "Cannon_Base" in the tank.
		 * This script controls the loading circle displyaed while the bullet is reloaded.
		 * This script works in combination with "Cannon_Fire_CS" in the tank.
		*/
        [SerializeField] private ReloadingCirclePresenter _reloadMarkerCanvasPrefab;
        [SerializeField] private Cannon_Fire_CS _cannonFireScript;

        private ReloadingCirclePresenter _reloadingCircle;

        private bool isSelected;
        private bool _isLoadeng;

        private void Start()
        {
            InitScript();
        }

        private void InitScript()
        {
            _reloadingCircle = Instantiate(_reloadMarkerCanvasPrefab);

            _cannonFireScript.OnReload.AddListener(ReloadLaunch);
            _cannonFireScript.OnEndReload.AddListener(ReloadEnd);
        }

        private void ReloadLaunch()
        {
            _reloadingCircle.EnableImage(true);
            _isLoadeng = true;
        }
        private void ReloadEnd()
        {
            _reloadingCircle.EnableImage(false);
            _isLoadeng = false;
        }

        void LateUpdate()
        {
            if (isSelected == false)
            {
                return;
            }

            if (_isLoadeng)
            {
                FillCircle();
            }
        }

        void FillCircle()
        {
            _reloadingCircle.FillCircle(_cannonFireScript.Loading_Count, _cannonFireScript.Reload_Time);
        }


        void Selected(bool isSelected)
        { // Called from "ID_Settings_CS".
            if (isSelected)
            {
                this.isSelected = true;
            }
            else
            {
                if (this.isSelected)
                { // This tank is selected until now.
                    this.isSelected = false;
                }
            }
        }


        void Turret_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS".

            Destroy(this);
        }


        void Pause(bool isPaused)
        { // Called from "Game_Controller_CS".
            this.enabled = !isPaused;
        }

    }

}