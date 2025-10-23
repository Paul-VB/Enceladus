using Enceladus.Core.Rendering;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Input
{
    public interface IInputReader
    {
        bool IsKeyPressed(KeyboardKey key);
        bool IsKeyDown(KeyboardKey key);
        bool IsKeyReleased(KeyboardKey key);
        bool GetFireWeaponInput();
        Vector2 GetMovementInput();
        Vector2 GetArrowKeyMovementInput();
        Vector2 GetMouseWorldPosition();
    }

    public class InputReader : IInputReader
    {
        private readonly ICameraManager _cameraManager;

        public InputReader(ICameraManager cameraManager)
        {
            _cameraManager = cameraManager;
        }

        public bool IsKeyPressed(KeyboardKey key) => Raylib.IsKeyPressed(key);

        public bool IsKeyDown(KeyboardKey key) => Raylib.IsKeyDown(key);

        public bool IsKeyReleased(KeyboardKey key) => Raylib.IsKeyReleased(key);

        public bool GetFireWeaponInput()
        {
            return Raylib.IsMouseButtonDown(KnownMouseControls.FireWeapon) ||
                   Raylib.IsKeyDown(KnownKeyboardControls.FireWeapon);
        }

        public Vector2 GetMovementInput()
        {
            var movement = Vector2.Zero;

            if (Raylib.IsKeyDown(KnownKeyboardControls.MoveUp))
                movement.Y -= 1f;
            if (Raylib.IsKeyDown(KnownKeyboardControls.MoveDown))
                movement.Y += 1f;
            if (Raylib.IsKeyDown(KnownKeyboardControls.MoveLeft))
                movement.X -= 1f;
            if (Raylib.IsKeyDown(KnownKeyboardControls.MoveRight))
                movement.X += 1f;

            if (movement.Length() > 0)
                movement = Vector2.Normalize(movement);

            return movement;
        }

        public Vector2 GetArrowKeyMovementInput()
        {
            var movement = Vector2.Zero;

            if (Raylib.IsKeyDown(KeyboardKey.Up))
                movement.Y -= 1f;
            if (Raylib.IsKeyDown(KeyboardKey.Down))
                movement.Y += 1f;
            if (Raylib.IsKeyDown(KeyboardKey.Left))
                movement.X -= 1f;
            if (Raylib.IsKeyDown(KeyboardKey.Right))
                movement.X += 1f;

            if (movement.Length() > 0)
                movement = Vector2.Normalize(movement);

            return movement;
        }

        public Vector2 GetMouseWorldPosition()
        {
            var mouseScreenPos = Raylib.GetMousePosition();
            return Raylib.GetScreenToWorld2D(mouseScreenPos, _cameraManager.Camera);
        }
    }

    public static class KnownKeyboardControls
    {
        public const KeyboardKey MoveUp = KeyboardKey.W;
        public const KeyboardKey MoveDown = KeyboardKey.S;
        public const KeyboardKey MoveLeft = KeyboardKey.A;
        public const KeyboardKey MoveRight = KeyboardKey.D;
        public const KeyboardKey Brake = KeyboardKey.LeftControl;
        public const KeyboardKey FireWeapon = KeyboardKey.Space;
        public const KeyboardKey Pause = KeyboardKey.Escape;
    }

    public static class KnownMouseControls
    {
        public const MouseButton FireWeapon = MouseButton.Left;
    }
}