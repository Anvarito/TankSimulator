using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

namespace ChobiAssets.PTM
{

    [DefaultExecutionOrder(+3)] // (Note.) This script is executed after the main camera is moved, in order to move the marker smoothly.
    public class LeadMarkerUIReceiver : MonoBehaviour
    {
        /*
		 * This script is attached to the "MainBody" of the tank with "Aiming_Control_CS".
		 * This script controls the Lead Marker in the scene.
		*/

        // User options >>
        [SerializeField] private LeadMarkerPresenter _leadMarkerPresenterPrefab;
        [SerializeField] private Aiming_Control_CS _aimingScript;
        [SerializeField] private Bullet_Generator_CS _bulletGenerator;
        [SerializeField] private Camera _camera;
        // << User options
        private LeadMarkerPresenter _leadMarkerPresenter;

        bool isSelected;

        void Start()
        {
            if (_aimingScript == null)
            {
                Debug.LogError("'Aiming_Control_CS' cannot be found in the MainBody.");
                Destroy(this);
            }
            if (_bulletGenerator == null)
            {
                Debug.LogError("'Bullet_Generator_CS' cannot be found. The cannon cannot get the bullet velocity.");
                Destroy(this);
                return;
            }

            if (_leadMarkerPresenterPrefab != null)
            {
                _leadMarkerPresenter = Instantiate(_leadMarkerPresenterPrefab);
                _leadMarkerPresenter.Initializing(_aimingScript, _bulletGenerator, _camera);
            }
            else
            {
                Debug.LogError("'_leadMarkerPresenterPrefab' cannot be found!!!");
                Destroy(this);
                return;
            }
        }


        void LateUpdate()
        {
            if (isSelected == false)
            {
                return;
            }

            _leadMarkerPresenter.MarkerControl();
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
                   // markerImage.enabled = false;
                }
            }
        }


        void MainBody_Destroyed_Linkage()
        { // Called from "Damage_Control_Center_CS".

            // Turn off the marker.
            if (isSelected)
            {
                //markerImage.enabled = false;
            }

            Destroy(this);
        }


        void Pause(bool isPaused)
        { // Called from "Game_Controller_CS".
            this.enabled = !isPaused;
        }

    }

}
