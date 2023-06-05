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
        public Collider Body_collider;
    }


    [DefaultExecutionOrder(+2)] // (Note.) This script is executed after the "Camera_Points_Manager_CS", in order to move the marker smoothly.
    public class PositionActorsMarkerRecivier : UIRecivierBase
    {
        /*
		 * This script is attached to the top object of the tank.
		 * This script controls the 'Position_Marker' in the scene.
		 * This script works in combination with "ID_Settings_CS" and "AI_CS" in the tank.
		*/


        // User options >>
        [SerializeField] private Color Friend_Color = Color.blue;
        [SerializeField] private Color Hostile_Color = Color.red;
        [SerializeField] private bool Show_Always = true;
        // << User options

        private List<Drive_Control_CS> _driveControlList = new List<Drive_Control_CS>();
        private Dictionary<Drive_Control_CS, Position_Marker_Prop> _markerDictionary = new Dictionary<Drive_Control_CS, Position_Marker_Prop>();

        private Camera _mainCamera;
        private ActorPointerUIHelper _markerCanvasUIHelper;
        private Canvas _canvas;

        private float Upper_Offset = 2f;
        Plane[] _cameraPlanes;
        private Vector2 localPoint;
        private ID_Settings_CS _IDSettings;
        private List<ID_Settings_CS> _enemysID;

        public void Init(ID_Settings_CS IDSettings, List<ID_Settings_CS> enemysID, Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup)
        {
            _gunCamera = gunCamera;
            _cameraSetup = cameraSetup;
            _IDSettings = IDSettings;
            _enemysID = enemysID;

            _mainCamera = _cameraSetup.GetCamera();

            InitialUIRecivier();
        }
        protected override void InstantiateCanvas()
        {
            base.InstantiateCanvas();
            _markerCanvasUIHelper = _spawnedPresenter as ActorPointerUIHelper;
            _canvas = _markerCanvasUIHelper.GetCanvas();

            StartCoroutine(SearchAllUits());

        }

        protected override void SwitchCamera(EActiveCameraType activeCamera)
        {
            base.SwitchCamera(activeCamera);
            foreach (var i in _driveControlList)
            {
                var imageGO = _markerDictionary[i].ArrowImage;
                if (imageGO != null)
                    imageGO.enabled = activeCamera == EActiveCameraType.MainCamera;
            }
        }

        private IEnumerator SearchAllUits()
        {
            yield return null;
            //temp!!! Add all units in list
            foreach (var idScript in _enemysID)
            {
                Drive_Control_CS currentActor = idScript.GetComponentInChildren<Drive_Control_CS>();
                _driveControlList.Add(currentActor);
                Receive_ID_Script(idScript, currentActor);
            }
        }

        void Receive_ID_Script(ID_Settings_CS idSetting, Drive_Control_CS driveControl)
        {
            Image arrowImage = Instantiate(_markerCanvasUIHelper.ArrowImage, _canvas.transform);
            Position_Marker_Prop newProp = new Position_Marker_Prop();
            newProp.ArrowImage = arrowImage;
            newProp.Marker_Transform = newProp.ArrowImage.transform;
            newProp.Root_Transform = idSetting.transform;
            newProp.Body_Transform = idSetting.GetComponentInChildren<Rigidbody>().transform;
            newProp.Body_collider = newProp.Body_Transform.GetComponent<Collider>();
            _markerDictionary.Add(driveControl, newProp);
            VisualizeMarker(newProp, idSetting.Relationship);

            newProp.ArrowImage.enabled = true;
        }


        void Update()
        {
            if (_canvas == null)
                return;

            Control_Markers();
        }

        protected override void DestroyUI()
        {
            Destroy(_canvas.gameObject); //Destroy todo
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
                //Debug.DrawLine(transform.position, currentActor.transform.position + Vector3.up * Upper_Offset, Color.white);
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
                if (GeometryUtility.TestPlanesAABB(_cameraPlanes, currentMarkerProp.Body_collider.bounds))
                {
                    indexPlane = -1;
                }
                 Vector3 worlsPoint = ray.GetPoint(minDistance);

                currentMarkerProp.ArrowImage.rectTransform.position = _mainCamera.WorldToScreenPoint(worlsPoint);
                //Debug.DrawLine(new Vector3(_mainCamera.pixelHeight / 2, _mainCamera.pixelWidth / 2), currentMarkerProp.ArrowImage.rectTransform.position, Color.blue);
                //var screenPoint = _mainCamera.WorldToScreenPoint(worlsPoint);
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPoint, _mainCamera, out localPoint);
                //currentMarkerProp.ArrowImage.rectTransform.anchoredPosition = localPoint;

                //Vector2 adjustedPosition = _mainCamera.WorldToScreenPoint(worlsPoint);

                //adjustedPosition.x *= _canvasRect.rect.width / (float)_mainCamera.pixelWidth;
                //adjustedPosition.y *= _canvasRect.rect.height / (float)_mainCamera.pixelHeight;

                //// set it
                //currentMarkerProp.ArrowImage.rectTransform.anchoredPosition = adjustedPosition;

                currentMarkerProp.ArrowImage.rectTransform.localRotation = SetRotationByIndex(indexPlane);
            }
        }

        public static Vector2 GetRelativePosOfWorldPoint(Vector3 worldPoint, Camera camera)
        {
            Vector3 screenPoint = camera.WorldToScreenPoint(worldPoint);
            return new Vector2(screenPoint.x / camera.pixelWidth, screenPoint.y / camera.pixelHeight);
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
            bool isAlly = _IDSettings.Relationship == relationship;
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