﻿using System.Collections;
using UnityEngine;

namespace ChobiAssets.PTM
{
    public enum ERelationship
    {
        TeamA,
        TeamB,
        Landmark
    }

    public enum EPlayerType
    {
        AI,
        Player
    }

    [DefaultExecutionOrder(+1)] // (Note.) This script must be executed after all the scripts executed the initializing.
    public class ID_Settings_CS : MonoBehaviour
    {
        /*
		 * This script is attached to the top object of the tank.
		 * This script is used for giving an individual ID number to the tank. 
		 * When "ID_Settings_CS" script is put in the scene, this script can receive a proper ID number from it even if the number was overlapping with other tanks.
		*/


        // User options >>
        [SerializeField, Range (0, 30)]private int _tankID = 1; // "0" = Not selectable.
        [SerializeField] private ERelationship _relationship;
        [SerializeField] private EPlayerType _playerType;

        //[Header("Initilize input scripts")]
        // << User options
        public EPlayerType PlayerType => _playerType;
        public bool IsSelected { get; private set; } // Referred to from "UI_PosMarker_Control_CS", "RC_Camera_CS".
        int currentID = 1;

        public ERelationship Relationship => _relationship;
        public int Tank_ID 
        { 
            get 
            { 
                return _tankID; 
            }
            set
            {
                Tank_ID = value;
            }
        }


        void Start()
        {
            Initialize();
        }


        void Initialize()
        { // This function must be called in Start(). Because this variables might be overwritten by "Event_Controller_CS" in the Awake().

            // Send message to the "Game_Controller" with "ID_Manager_CS" in the scene to add this tank to the list.
            if (ID_Manager_CS.Instance)
            {
                // Also, the proper ID is set by "ID_Manager_CS",
                ID_Manager_CS.Instance.SendMessage("Receive_ID_Script" ,this, SendMessageOptions.DontRequireReceiver);
            }

            // Set the "Is_Selected" value.
            IsSelected = (Tank_ID == currentID);

            // Broadcast whether this tank is selected or not.
            Broadcast_Selection_Condition();


            //_driveControl.Initialize();
        }

        public void Receive_Current_ID(int currentID)
        { // Called from "ID_Manager_CS" in the scene, when the player changes the tank.
            this.currentID = currentID;
            IsSelected = (Tank_ID == currentID);
            Broadcast_Selection_Condition();
        }


        void Broadcast_Selection_Condition()
        {
            // Broadcast whether this tank is selected or not to all the children.
            BroadcastMessage("Selected", IsSelected, SendMessageOptions.DontRequireReceiver);
        }


        void Respawned()
        { // Called from "Respawn_Controller_CS", when the tank has been respawned.
            // Broadcast whether this tank is selected or not.
            Broadcast_Selection_Condition();
        }


        void Prepare_Removing()
        { // Called from "Respawn_Controller_CS", just before the tank is removed.

            // Send message to the "Game_Controller" with "ID_Manager_CS" in the scene to remove this tank from the lists.
            if (ID_Manager_CS.Instance)
            {
                ID_Manager_CS.Instance.SendMessage("Remove_ID", this, SendMessageOptions.DontRequireReceiver);
            }
        }

    }

}
