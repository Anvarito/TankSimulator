using System.Collections;
using System.Collections.Generic;
using ChobiAssets.PTM;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputIniializeManager))]
public class InputInitializeManager_Editor : Editor
{
    SerializedProperty IDSettings_prop;
    SerializedProperty DamageManager_prop;
    SerializedProperty AI_CS_prop;
    SerializedProperty Drive_Control_CS_prop;
    SerializedProperty Cannon_Fire_CS_prop;
    SerializedProperty Aiming_Control_CS_prop;
    SerializedProperty Camera_Points_Manager_CS_prop;
    SerializedProperty Gun_Camera_CS_prop;
    SerializedProperty Camera_Rotation_CS_prop;
    private Transform thisTransform;
    private ID_Settings_CS _iD_Settings_CS;
    void OnEnable()
    {
        IDSettings_prop = serializedObject.FindProperty("_iDSettingsCS");
        DamageManager_prop = serializedObject.FindProperty("_damageManager");
        AI_CS_prop = serializedObject.FindProperty("ai_core_script");
        Drive_Control_CS_prop = serializedObject.FindProperty("_driveControl");
        Cannon_Fire_CS_prop = serializedObject.FindProperty("_fireControl");
        Aiming_Control_CS_prop = serializedObject.FindProperty("_aimingControl");
        Camera_Points_Manager_CS_prop = serializedObject.FindProperty("_cameraPointsControl");
        Gun_Camera_CS_prop = serializedObject.FindProperty("_gunCameraControl");
        Camera_Rotation_CS_prop = serializedObject.FindProperty("_cameraRotation");

        if (Selection.activeGameObject)
        {
            thisTransform = Selection.activeGameObject.transform;
            _iD_Settings_CS = thisTransform.GetComponent<ID_Settings_CS>();
        }
    }

    public override void OnInspectorGUI()
    {
        Set_Inspector();
    }


    void Set_Inspector()
    {
        serializedObject.Update();

        GUI.backgroundColor = new Color(1.0f, 1.0f, 0.5f, 1.0f);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Main links", MessageType.None, true);
        IDSettings_prop.objectReferenceValue = EditorGUILayout.ObjectField("ID Settings", IDSettings_prop.objectReferenceValue, typeof(ID_Settings_CS), true);
        DamageManager_prop.objectReferenceValue = EditorGUILayout.ObjectField("Damage reciviers manager", DamageManager_prop.objectReferenceValue, typeof(DamageReciviersManager), true);

        EditorGUILayout.Space();
        if (_iD_Settings_CS.PlayerType == EPlayerType.AI)
        {
            EditorGUILayout.HelpBox("AI core links", MessageType.Info, true);
            AI_CS_prop.objectReferenceValue = EditorGUILayout.ObjectField("AI_CS script", AI_CS_prop.objectReferenceValue, typeof(AI_CS), true);
        }
        else
        {
            EditorGUILayout.HelpBox("Player links", MessageType.Info, true);
            Camera_Points_Manager_CS_prop.objectReferenceValue = EditorGUILayout.ObjectField("Camera points manager", Camera_Points_Manager_CS_prop.objectReferenceValue, typeof(Camera_Points_Manager_CS), true);
            Gun_Camera_CS_prop.objectReferenceValue = EditorGUILayout.ObjectField("Gun camera control", Gun_Camera_CS_prop.objectReferenceValue, typeof(Gun_Camera_CS), true);
            Camera_Rotation_CS_prop.objectReferenceValue = EditorGUILayout.ObjectField("Camera rotation", Camera_Rotation_CS_prop.objectReferenceValue, typeof(Camera_Rotation_CS), true);
        }

        Aiming_Control_CS_prop.objectReferenceValue = EditorGUILayout.ObjectField("Aiming control", Aiming_Control_CS_prop.objectReferenceValue, typeof(Aiming_Control_CS), true);
        Drive_Control_CS_prop.objectReferenceValue = EditorGUILayout.ObjectField("Drive control", Drive_Control_CS_prop.objectReferenceValue, typeof(Drive_Control_CS), true);
        Cannon_Fire_CS_prop.objectReferenceValue = EditorGUILayout.ObjectField("Canon FIre", Cannon_Fire_CS_prop.objectReferenceValue, typeof(Cannon_Fire_CS), true);

        serializedObject.ApplyModifiedProperties();
    }
}
