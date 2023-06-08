using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Gizmos_Debug;
using Infrastructure.Services.StaticData.WaypointsPack;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Editor.Inspector
{
    [CustomEditor(typeof(WaypointPacksData))]
    public class CustomWaypointPacksData : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            WaypointPacksData data = (WaypointPacksData)target;

            if (GUILayout.Button("Save"))
            {
                WaypointsVisualDebug[] packs = FindObjectsOfType<WaypointsVisualDebug>();

                data.Packs ??= new List<WaypointPackConfig>();
                data.Packs.Clear();

                foreach (WaypointsVisualDebug pack in packs)
                    data.Packs.Add(CompositeConfig(pack));
            }

            if (GUILayout.Button("Load"))
            {
                if (data.Packs == null || data.Packs.Count < 1)
                {
                    Debug.Log("Fill the list of waypoints ");
                    return;
                }

                Transform tempParent = null;
                    
                try
                {
                    WaypointsVisualDebug[] packs = FindObjectsOfType<WaypointsVisualDebug>();
                    tempParent = packs?.First().transform.parent;

                    foreach (WaypointsVisualDebug pack in packs)
                        DestroyImmediate(pack.gameObject);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                foreach (WaypointPackConfig packConfig in data.Packs)
                    InstantiatePack(packConfig, tempParent);
            }
        }

        private static void InstantiatePack(WaypointPackConfig packConfig, Transform tempParent)
        {
            string name = Enum.GetName(typeof(WaypointsPackId), packConfig.PackId);

            GameObject packObject = new GameObject(name);
            packObject.transform.SetParent(tempParent);

            packObject.AddComponent<WaypointsVisualDebug>().PackId = packConfig.PackId;

            foreach (var point in packConfig.Points.Select((value, index) => new { position = value, index }))
            {
                Transform pointObject = new GameObject("Waypoint " + point.index).transform;
                pointObject.position = point.position;
                pointObject.SetParent(packObject.transform);
            }
        }

        private static WaypointPackConfig CompositeConfig(WaypointsVisualDebug pack)
        {
            WaypointPackConfig config = new WaypointPackConfig();

            List<Vector3> points = new List<Vector3>();
            foreach (Transform point in pack.GetComponentsInChildren<Transform>())
                if (point != pack.transform) points.Add(point.position);

            config.Points = points;
            config.PackId = pack.PackId;

            return config;
        }
    }
}