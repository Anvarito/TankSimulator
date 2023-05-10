using UnityEngine;

namespace Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        public Vector2 Move =>
            GetMove();

        private static Vector2 GetMove() => 
            HorizontalMove() + VerticalMove();

        private static Vector2 VerticalMove() => 
            UnityEngine.Input.GetAxis("Vertical") * Vector2.up;

        private static Vector2 HorizontalMove() => 
            UnityEngine.Input.GetAxis("Horizontal") * Vector2.right;
    }
}