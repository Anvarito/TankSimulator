﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

    [CustomEditor(typeof(DamageTrackPieceRecivier))]
    public class Damage_Control_04_Track_Collider_CSEditor : Editor
    {

        //SerializedProperty _initHP;
        SerializedProperty Linked_Piece_ScriptProp;
        //SerializedProperty Damage_treshold;

        // SerializedProperty Has_ChangedProp;

        void OnEnable()
        {
            //_initHP = serializedObject.FindProperty("_InitialTrackHP");
            Linked_Piece_ScriptProp = serializedObject.FindProperty("Linked_Piece_Script");
            //Damage_treshold = serializedObject.FindProperty("_damageThreshold");

            // Has_ChangedProp = serializedObject.FindProperty("Has_Changed");
            //_isRepairable = serializedObject.FindProperty("_repairableTrack");
        }


        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            serializedObject.Update();
            GUI.backgroundColor = new Color(1.0f, 1.0f, 0.5f, 1.0f);

            //EditorGUILayout.Space ();
            //EditorGUILayout.Space ();
            if (GUILayout.Button("Find the closest piece", GUILayout.Width(200)))
            {
                Find_Closest_Piece();
            }

            EditorGUILayout.Space();
            //_initHP.floatValue = EditorGUILayout.FloatField("Initital HP", _initHP.floatValue);
            //EditorGUILayout.Slider(Damage_treshold, 0.0f, 1000.0f, "Damage treshold");
            
            //Linked_Piece_ScriptProp.objectReferenceValue = EditorGUILayout.ObjectField ("Linked Piece", Linked_Piece_ScriptProp.objectReferenceValue, typeof(Static_Track_Piece_CS), true);
            //EditorGUILayout.Space ();

            //if (Track_IndexProp.intValue == 0) { // Left
            //	EditorGUILayout.HelpBox("Left side", MessageType.None, false);
            //}
            //else {
            //	EditorGUILayout.HelpBox("Right side", MessageType.None, false);
            //}
            //EditorGUILayout.Space ();

            //         if (GUI.changed)
            //         {
            //             Has_ChangedProp.boolValue = !Has_ChangedProp.boolValue;
            //         }

            serializedObject.ApplyModifiedProperties();
        }


        void Find_Closest_Piece()
        {
            Transform thisTransform = Selection.activeGameObject.transform;

            // Find the closest piece.
            Static_Track_Piece_CS[] pieceScripts = thisTransform.parent.parent.GetComponentsInChildren<Static_Track_Piece_CS>();
            if (pieceScripts.Length == 0)
            { // This track might be a "Scroll_Track".
                Debug.Log("The 'Linked Piece' is not necessary for 'Scroll_Track'. The script has set only the direction.");
                // Set the direction.
                //Set_Direction(false);
                return;
            }

            float minDist = Mathf.Infinity;
            Static_Track_Piece_CS closestPieceScript = null;
            foreach (Static_Track_Piece_CS pieceScript in pieceScripts)
            {
                float tempDist = Vector3.Distance(thisTransform.position, pieceScript.transform.position);
                if (tempDist < minDist)
                {
                    minDist = tempDist;
                    closestPieceScript = pieceScript;
                }
            }
            if (closestPieceScript == null)
            {
                Debug.LogWarning("The closest piece cannot be found.");
                return;
            }
            Linked_Piece_ScriptProp.objectReferenceValue = closestPieceScript as Static_Track_Piece_CS;

            // Set the position
            Transform closestPieceTransform = closestPieceScript.transform;
            Vector3 pos = thisTransform.position;
            pos.x = closestPieceTransform.position.x;
            pos.z = (closestPieceTransform.position - (closestPieceTransform.transform.up * 0.1f)).z;
            thisTransform.position = pos;

            // Set the rotation
            Vector3 rot = thisTransform.localEulerAngles;
            rot.z = 0.0f;
            thisTransform.localEulerAngles = rot;

            // Set the scale.
            Vector3 tempScale = thisTransform.localScale;
            CapsuleCollider[] capsuleColliders = closestPieceTransform.GetComponents<CapsuleCollider>();
            for (int i = 0; i < capsuleColliders.Length; i++)
            {
                if (capsuleColliders[i].direction == 0)
                { // X-Axis.
                    if (thisTransform.parent.localEulerAngles.z == 90.0f)
                    { // in Static_Track
                        tempScale.y = capsuleColliders[i].height;
                    }
                    else
                    { // in Scroll_Track
                        tempScale.x = capsuleColliders[i].height;
                    }
                }
                else
                { // Z-Axis.
                    if (thisTransform.parent.localEulerAngles.z == 90.0f)
                    { // in Static_Track
                        tempScale.x = capsuleColliders[i].height * 2.0f;
                    }
                    else
                    { // in Scroll_Track
                        tempScale.y = capsuleColliders[i].height * 2.0f;
                    }
                }
            }
            thisTransform.localScale = tempScale;

            // Set the direction.
            //Set_Direction(true);
        }


        //void Set_Direction(bool forStaticTrack)
        //{
        //    Transform thisTransform = Selection.activeGameObject.transform;
        //    if (forStaticTrack == true)
        //    { // This Track_Collider is used for "Static_Track".
        //        if (thisTransform.localPosition.y > 0.0f)
        //        { // Left
        //            _initHP.intValue = 0;
        //        }
        //        else
        //        { // Right
        //            _initHP.intValue = 1;
        //        }
        //    }
        //    else
        //    { // This Track_Collider is used for "Scroll_Track".
        //        if (thisTransform.localPosition.x < 0.0f)
        //        { // Left
        //            _initHP.intValue = 0;
        //        }
        //        else
        //        { // Right
        //            _initHP.intValue = 1;
        //        }
        //    }
        //}

    }
}