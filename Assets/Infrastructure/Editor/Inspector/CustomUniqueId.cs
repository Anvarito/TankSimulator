using System;
using System.Linq;
using Infrastructure.TestMono;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Editor.Inspector
{
    [CustomEditor(typeof(UniqueId))]
    public class CustomUniqueId : UnityEditor.Editor
    {
        public void OnEnable()
        {
            UniqueId uniqueId = (UniqueId)target;
         
            if (IsPrefab(uniqueId))
                return;
            
            if (string.IsNullOrEmpty(uniqueId.Id))
            {
                Generate(uniqueId);
            }
            else
            {
                UniqueId[] points = FindObjectsOfType<UniqueId>();
                
                if (points.Any(other=>other!=uniqueId && other.Id == uniqueId.Id))
                    Generate(uniqueId);
            }
        }

        private bool IsPrefab(UniqueId spawnPoint) => 
            spawnPoint.gameObject.scene.rootCount == 0;

        private void Generate(UniqueId spawnPoint)
        {
            spawnPoint.Id = $"{SceneManager.GetActiveScene().name}_{Guid.NewGuid()}";

            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(spawnPoint);
                EditorSceneManager.MarkSceneDirty(spawnPoint.gameObject.scene);
            }
        }
    }
}