using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.TestMono;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Editor.Inspector
{
    [CustomEditor(typeof(SpawnPointPackData))]
    public class CustomSpawnPointPack : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SpawnPointPackData data = (SpawnPointPackData)target;

            if (GUILayout.Button("Save"))
            {
                SpawnMarker[] spawnPoints = FindObjectsOfType<SpawnMarker>();

                data.PackConfig.PointsConfigs ??= new List<SpawnPointConfig>();
                data.PackConfig.PointsConfigs.Clear();

                foreach (SpawnMarker point in spawnPoints)
                    data.PackConfig.PointsConfigs.Add(CompositeConfig(point));
                
                EditorUtility.SetDirty(data);
            }

            if (GUILayout.Button("Load"))
            {
                if (data.PackConfig.PointsConfigs == null || data.PackConfig.PointsConfigs.Count < 1)
                {
                    Debug.Log("Fill the list of spawn points");
                    return;
                }

                Transform tempParent = null;

                try
                {
                    SpawnMarker[] points = FindObjectsOfType<SpawnMarker>();
                    tempParent = points?.First().transform.parent;

                    foreach (SpawnMarker point in points)
                        DestroyImmediate(point.gameObject);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                foreach (SpawnPointConfig packConfig in data.PackConfig.PointsConfigs)
                    InstantiatePack(packConfig, tempParent);
                
                
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

        private static void InstantiatePack(SpawnPointConfig pointConfig, Transform tempParent)
        {
            string name = Enum.GetName(typeof(EPlayerType), pointConfig.ActorType);
            name += "SpawnMarker";
            name += Enum.GetName(typeof(ERelationship), pointConfig.Team);
            
            GameObject packObject = new GameObject(name);
            packObject.transform.SetParent(tempParent);
            packObject.transform.position = pointConfig.Position;

            packObject.AddComponent<UniqueId>().Id = pointConfig.UniqueId;
            
            SpawnMarker spawnMarker = packObject.AddComponent<SpawnMarker>();

            spawnMarker.ActorType = pointConfig.ActorType;
            spawnMarker.Relationship = pointConfig.Team;
            spawnMarker.WaypointsPackId = pointConfig.WaypointsPackId;
        }

        private static SpawnPointConfig CompositeConfig(SpawnMarker marker)
        {
            SpawnPointConfig config = new SpawnPointConfig();

            config.Position = marker.GetComponent<Transform>().position;
            //config.UniqueId = marker.GetComponent<UniqueId>().Id;
            
            config.Team = marker.Relationship;
            config.ActorType = marker.ActorType;
            config.WaypointsPackId = marker.WaypointsPackId;

            return config;
        }
    }
}