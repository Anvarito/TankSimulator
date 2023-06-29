using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlacerObject))]
[CanEditMultipleObjects]
public class PlacerObjectEditor : Editor
{
    SerializedProperty _listObjects;
    SerializedProperty _container;
    private GameObject thisGO;
    List<GameObject> trees;
    GameObject spawned;
    void OnEnable()
    {
        _listObjects = serializedObject.FindProperty("Trees");
        _container = serializedObject.FindProperty("Container");

        if (Selection.activeGameObject)
        {
            thisGO = Selection.activeGameObject;
            if (trees == null)
                trees = thisGO.GetComponent<PlacerObject>().Trees;
        }
    }
    void OnSceneGUI()
    {
        Transform container = _container.objectReferenceValue as Transform;
        if (Event.current.keyCode == KeyCode.R)
        {

            if (Event.current.rawType != EventType.KeyDown)
                return;

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.tag == "Tree")
                    return;

                if (spawned != null
                     && Vector3.Distance(spawned.transform.position, hit.transform.position) < 1.5f)
                    return;

                Vector3 newTilePosition = hit.point;
                spawned = Instantiate(GetRandomTree(), newTilePosition, Quaternion.identity);
                spawned.transform.parent = container;
            }
        }
    }

    private GameObject GetRandomTree()
    {
        int indx = Random.Range(0, trees.Count);
        return trees[indx];
    }
}
