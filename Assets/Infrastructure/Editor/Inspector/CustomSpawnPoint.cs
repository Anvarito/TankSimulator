using System;
using System.Collections.Generic;
using System.Linq;
using ChobiAssets.PTM;
using Infrastructure.Gizmos_Debug;
using Infrastructure.Services.StaticData.Level;
using Infrastructure.Services.StaticData.SpawnPoints;
using Infrastructure.Services.StaticData.WaypointsPack;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Editor.Inspector
{
    [CustomEditor(typeof(SpawnPointPackData))]
    public class CustomSpawnPoint : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SpawnPointPackData data = (SpawnPointPackData)target;

            if (GUILayout.Button("Save"))
            {
                SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();

                data.PackConfig.PointsConfigs ??= new List<SpawnPointConfig>();
                data.PackConfig.PointsConfigs.Clear();

                foreach (SpawnPoint point in spawnPoints)
                    data.PackConfig.PointsConfigs.Add(CompositeConfig(point));
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
                    SpawnPoint[] points = FindObjectsOfType<SpawnPoint>();
                    tempParent = points?.First().transform.parent;

                    foreach (SpawnPoint point in points)
                        DestroyImmediate(point.gameObject);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                foreach (SpawnPointConfig packConfig in data.PackConfig.PointsConfigs)
                    InstantiatePack(packConfig, tempParent);
            }
        }

        private static void InstantiatePack(SpawnPointConfig pointConfig, Transform tempParent)
        {
            string name = Enum.GetName(typeof(EPlayerType), pointConfig.ActorType);
            name += "SpawnPoint";
            name += Enum.GetName(typeof(ERelationship), pointConfig.Team);
            
            GameObject packObject = new GameObject(name);
            packObject.transform.SetParent(tempParent);
            packObject.transform.position = pointConfig.Position;

            SpawnPoint spawnPoint = packObject.AddComponent<SpawnPoint>();

            spawnPoint.ActorType = pointConfig.ActorType;
            spawnPoint.Relationship = pointConfig.Team;
            spawnPoint.WaypointsPackId = pointConfig.WaypointsPackId;
        }

        private static SpawnPointConfig CompositeConfig(SpawnPoint point)
        {
            SpawnPointConfig config = new SpawnPointConfig();

            config.Position = point.GetComponent<Transform>().position;
            config.Team = point.Relationship;
            config.ActorType = point.ActorType;
            config.WaypointsPackId = point.WaypointsPackId;

            return config;
        }
    }
}