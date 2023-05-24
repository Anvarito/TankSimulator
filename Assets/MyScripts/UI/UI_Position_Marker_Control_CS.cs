using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace ChobiAssets.PTM
{

    [System.Serializable]
    public class Position_Marker_Prop
    {
        public Image ArrowImage;
        public Transform Marker_Transform;
        public Transform Root_Transform;
        public Transform Body_Transform;
        public AI_CS AI_Script;
    }


    [DefaultExecutionOrder(+2)] // (Note.) This script is executed after the "Camera_Points_Manager_CS", in order to move the marker smoothly.
    public class UI_Position_Marker_Control_CS : MonoBehaviour
    {
        /*
		 * This script is attached to the top object of the tank.
		 * This script controls the 'Position_Marker' in the scene.
		 * This script works in combination with "ID_Settings_CS" and "AI_CS" in the tank.
		*/


        // User options >>
        [SerializeField] private ActorPointerUIHelper Marker_Prefab;
        [SerializeField] private Color Friend_Color = Color.blue;
        [SerializeField] private Color Hostile_Color = Color.red;
        [SerializeField] private bool Show_Always = true;
        // << User options

        private List<Drive_Control_CS> _driveControlList = new List<Drive_Control_CS>();
        private Dictionary<Drive_Control_CS, Position_Marker_Prop> _markerDictionary = new Dictionary<Drive_Control_CS, Position_Marker_Prop>();

        private float Upper_Offset = 3;
        private Camera _mainCamera;
        private CameraViewSetup _cameraViewSetup;
        private ID_Settings_CS _selfIdSetting;
        private Canvas _canvas;

        Plane[] _cameraPlanes;


        public void Initialize(CameraViewSetup cameraViewSetup, ID_Settings_CS iD_Settings_CS, Gun_Camera_CS gun_Camera_CS)
        {
            // Check the prefab.
            if (Marker_Prefab == null)
            {
                Debug.LogWarning("'Prefab for 'Position Maker' is not assigned.");
                Destroy(this);
                return;
            }

            gun_Camera_CS.OnSwitchCamera.AddListener(CameraSwitch);
            _cameraViewSetup = cameraViewSetup;
            _selfIdSetting = iD_Settings_CS;
            _mainCamera = _cameraViewSetup.GetCamera();

            // Set the canvas.
            SetCanvas();

            StartCoroutine(SearchAllUits());
        }

        private void CameraSwitch(EActiveCameraType typeCamera)
        {
            foreach (var i in _driveControlList)
            {
                var imageGO = _markerDictionary[i].ArrowImage;
                if (imageGO != null)
                    imageGO.enabled = typeCamera == EActiveCameraType.MainCamera;
            }
        }

        private IEnumerator SearchAllUits()
        {
            yield return null;
            //temp!!! Add all units in list
            foreach (var idScript in FindObjectsOfType<ID_Settings_CS>())
            {
                if (idScript == _selfIdSetting)
                    continue;
                Drive_Control_CS currentActor = idScript.GetComponentInChildren<Drive_Control_CS>();
                _driveControlList.Add(currentActor);
                Receive_ID_Script(idScript, currentActor);
            }
        }

        private void SetCanvas()
        {
            Canvas canvas = new GameObject("MARKER POSITION CANVAS").AddComponent<Canvas>();
            _canvas = canvas;
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        }

        void Receive_ID_Script(ID_Settings_CS idSetting, Drive_Control_CS driveControl)
        {
            var markerObject = Instantiate(Marker_Prefab, _canvas.transform);
            var newProp = new Position_Marker_Prop();
            newProp.ArrowImage = markerObject.ArrowImage;
            newProp.Marker_Transform = newProp.ArrowImage.transform;
            newProp.Root_Transform = idSetting.transform;
            newProp.Body_Transform = idSetting.GetComponentInChildren<Rigidbody>().transform;
            newProp.AI_Script = idSetting.GetComponentInChildren<AI_CS>();
            _markerDictionary.Add(driveControl, newProp);
            VisualizeMarker(newProp, idSetting.Relationship);

            newProp.ArrowImage.enabled = true;
        }


        void LateUpdate()
        {
            Control_Markers();
        }


        void Control_Markers()
        {
            for (int i = 0; i < _driveControlList.Count; i++)
            {
                Drive_Control_CS currentActor = _driveControlList[i];
                Position_Marker_Prop currentMarkerProp;

                if (!_markerDictionary.TryGetValue(currentActor, out currentMarkerProp))
                {
                    continue;
                }
                // Check the tank is selected now, or has been dead.
                if (currentMarkerProp.Root_Transform.tag == Layer_Settings_CS.FinishTag)
                {
                    Remove_ID(currentActor);
                    continue;
                }

                Vector3 playerToEnemy = (currentActor.transform.position + Vector3.up * Upper_Offset) - transform.position;
                Ray ray = new Ray(transform.position, playerToEnemy.normalized);
                _cameraPlanes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);

                float minDistance = float.MaxValue;
                int indexPlane = 0;

                for (int j = 0; j < _cameraPlanes.Length; j++)
                {
                    if (_cameraPlanes[j].Raycast(ray, out float distance))
                    {
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            indexPlane = j;
                        }
                    }
                }

                minDistance = Mathf.Clamp(minDistance, 0, playerToEnemy.magnitude);
                Vector3 worlsPoint = ray.GetPoint(minDistance);
                currentMarkerProp.ArrowImage.rectTransform.position = _mainCamera.WorldToScreenPoint(worlsPoint);
                currentMarkerProp.ArrowImage.transform.localRotation = SetRotationByIndex(indexPlane);
            }
        }
        private Quaternion SetRotationByIndex(int index)
        {
            int zRot = 0;
            if (index == 0) zRot = -90;
            if (index == 1) zRot = 90;
            if (index == 2) zRot = 0;
            if (index == 3) zRot = 180;

            return Quaternion.Euler(0, 0, zRot);
        }

        private void VisualizeMarker(Position_Marker_Prop currentMarkerProp, ERelationship relationship)
        {
            bool isAlly = _selfIdSetting.Relationship == relationship;
            currentMarkerProp.ArrowImage.enabled = Show_Always;
            currentMarkerProp.Marker_Transform.localScale = Vector3.one * 1.5f;
            currentMarkerProp.ArrowImage.color = isAlly ? Friend_Color : Hostile_Color;
        }

        void Remove_ID(Drive_Control_CS driveControl)
        { // Called from "ID_Settings_CS", just before the tank is removed from the scene.

            // Destroy the marker.
            Destroy(_markerDictionary[driveControl].ArrowImage.gameObject);

            // Remove the "ID_Settings_CS" from the list.
            _driveControlList.Remove(driveControl);

            // Remove the components from the dictionary.
            _markerDictionary.Remove(driveControl);
        }

    }

}