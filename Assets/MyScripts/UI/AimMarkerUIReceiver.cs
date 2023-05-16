using UnityEngine;


namespace ChobiAssets.PTM
{

    [DefaultExecutionOrder(+2)] // (Note.) This script is executed after the "Aiming_Control_CS", in order to move the marker smoothly.
    public class AimMarkerUIReceiver : MonoBehaviour
    {
        /*
		 * This script is attached to the "MainBody" of the tank.
		 * This script controls the 'Aim_Marker' in the scene.
         * This script works in combination with "Aiming_Control_CS" in the tank.
		*/

        // User options >>
        [SerializeField] private AimMarkerPresenter _aimLeadPresenterPrefab;
        [SerializeField] private Aiming_Control_CS aimingScript;
        [SerializeField] private Camera _camera;
        // << User options
        private AimMarkerPresenter _aimLeadPresenter;

        bool isSelected;

        public void Initialize(Aiming_Control_CS partsAiming)
        {
            aimingScript = partsAiming;
            isSelected = true;
        }


        void Start()
        {
            // Get the "Aiming_Control_CS" in the tank.
            if (aimingScript == null)
            {
                Debug.LogWarning("'Aiming_Control_CS' cannot be found in the MainBody.");
                Destroy(this);
            }
            // Get the marker image in the scene.
            if (_aimLeadPresenterPrefab == null)
            {
                return;
            }
            else
            {
                _aimLeadPresenter = Instantiate(_aimLeadPresenterPrefab);
            }

            _aimLeadPresenter.Initializing(aimingScript, _camera);
        }


        void Update()
        {
            if (isSelected == false)
            {
                return;
            }

            if (_aimLeadPresenter != null)
                _aimLeadPresenter.AimMarkerControl();
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
                    //markerImage.enabled = false;
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
