using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(AdditionalDamageZone))]
	public class Damage_Control_09_Armor_Collider_CSEditor : Editor
	{
	
		SerializedProperty Damage_MultiplierProp;
		SerializedProperty Damage_treshold;
		SerializedProperty HideVisual;


		void OnEnable ()
		{
			Damage_MultiplierProp = serializedObject.FindProperty ("Damage_Multiplier");
			Damage_treshold = serializedObject.FindProperty ("_damageThreshold");
			HideVisual = serializedObject.FindProperty ("_isShowVisual");
		}


		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();
			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
			EditorGUILayout.Space ();

			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Damage Multiplier settings", MessageType.None, true);
			HideVisual.boolValue = EditorGUILayout.Toggle("Show visual on start?", HideVisual.boolValue);
			EditorGUILayout.Slider (Damage_MultiplierProp, 0.0f, 10.0f, "Damage Multiplier");
			//EditorGUILayout.Slider (Damage_treshold, 0.0f, 1000.0f, "Damage treshold");


			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();
		}

	}

}