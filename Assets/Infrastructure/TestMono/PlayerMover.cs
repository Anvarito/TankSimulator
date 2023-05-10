using System;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Infrastructure.Services.Progress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class PlayerMover : MonoBehaviour, IProgressWriter
    {
        private IInputService _input;
        [SerializeField] private CharacterController controller;

        private void Awake()
        {
            _input = ServiceLocator.Container.Single<IInputService>();
        }


        private void Update()
        {
            controller.Move(MoveVector() * (5 * Time.deltaTime));
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel =
                new PositionOnLevel(CurrentLevelName(), transform.position.ToVectorData());
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevelName() == progress.WorldData.PositionOnLevel.Level)
            {
                VectorData savedPosition = progress.WorldData.PositionOnLevel.Position;
                
                if (savedPosition != null) 
                    Warp(to: savedPosition);
            }
        }

        private void Warp(VectorData to)
        {
            controller.enabled = false;
            transform.position = to.AsUnityVector();
            controller.enabled = true;
        }

        private Vector3 MoveVector()
        {
            Vector3 move = Vector3.zero;

            if (_input.Move.magnitude > 0.01)
            {
                move = Camera.main.transform.TransformDirection(InputToVector3());
                move.y = 0;
            }

            move += Physics.gravity;
            return move;
        }

        private Vector3 InputToVector3() =>
            Vector3.right * _input.Move.x + Vector3.forward * _input.Move.y;

        private static string CurrentLevelName() =>
            SceneManager.GetActiveScene().name;
    }
}