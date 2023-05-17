using UnityEngine;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Cannon_Fire_CS))]
	public class Cannon_Fire_CSEditor : Editor
	{
	
		SerializedProperty Reload_TimeProp;
		SerializedProperty Recoil_ForceProp;

		SerializedProperty _recoil_Brake_CSPROP;
		SerializedProperty _bullet_Generator_PROP;


		void OnEnable ()
		{
			Reload_TimeProp = serializedObject.FindProperty ("Reload_Time");
			Recoil_ForceProp = serializedObject.FindProperty ("Recoil_Force");

			_recoil_Brake_CSPROP = serializedObject.FindProperty ("_recoil_Brake_CS");
			_bullet_Generator_PROP = serializedObject.FindProperty ("_bullet_Generator_Script");
		}

		public override void  OnInspectorGUI ()
		{
			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
			serializedObject.Update ();

			_recoil_Brake_CSPROP.objectReferenceValue = EditorGUILayout.ObjectField("Recoil brake", _recoil_Brake_CSPROP.objectReferenceValue, typeof(Recoil_Brake_CS), true);
			_bullet_Generator_PROP.objectReferenceValue = EditorGUILayout.ObjectField("Bullet generator", _bullet_Generator_PROP.objectReferenceValue, typeof(Bullet_Generator_CS), true);

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Firing settings", MessageType.None, true);
			EditorGUILayout.Slider (Reload_TimeProp, 0.01f, 60.0f, "Reload Time");
			EditorGUILayout.Slider (Recoil_ForceProp, 0.0f, 30000.0f, "Recoil Force");
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		
			serializedObject.ApplyModifiedProperties ();
		}

	}

}