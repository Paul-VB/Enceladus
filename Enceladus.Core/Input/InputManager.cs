using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Input
{
    public interface IInputManager
    {
        bool IsKeyPressed(KeyboardKey key);
        bool IsKeyDown(KeyboardKey key);
        bool IsKeyReleased(KeyboardKey key);
        Vector2 GetMovementInput();
        Vector2 GetArrowKeyMovementInput();
    }

    public class InputManager : IInputManager
    {
        public bool IsKeyPressed(KeyboardKey key) => Raylib.IsKeyPressed(key);

        public bool IsKeyDown(KeyboardKey key) => Raylib.IsKeyDown(key);

        public bool IsKeyReleased(KeyboardKey key) => Raylib.IsKeyReleased(key);

        public Vector2 GetMovementInput()
        {
            var movement = Vector2.Zero;

            if (Raylib.IsKeyDown(KnownInputControls.MoveUp))
                movement.Y -= 1f;
            if (Raylib.IsKeyDown(KnownInputControls.MoveDown))
                movement.Y += 1f;
            if (Raylib.IsKeyDown(KnownInputControls.MoveLeft))
                movement.X -= 1f;
            if (Raylib.IsKeyDown(KnownInputControls.MoveRight))
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
    }

    public static class KnownInputControls
    {
        public const KeyboardKey MoveUp = KeyboardKey.W;
        public const KeyboardKey MoveDown = KeyboardKey.S;
        public const KeyboardKey MoveLeft = KeyboardKey.A;
        public const KeyboardKey MoveRight = KeyboardKey.D;
        public const KeyboardKey Pause = KeyboardKey.Escape;
    }
}