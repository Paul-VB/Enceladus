using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Entities
{
    //todo: Component-based architecture - Track progress and future work
    //
    //      ✅ COMPLETED - Interface cleanup:
    //        ✅ IMovable interface exists (Physics.Motion namespace) with complex methods (Accelerate, ApplyTorque)
    //        ✅ ICollidable interface exists (Physics.Collision namespace) with simple properties only
    //        ✅ Removed redundant interfaces (IEntity, ICollidableEntity, IPlayer)
    //        ✅ MovableComponent exists - delegates complex physics logic (avoids diamond problem)
    //        ✅ MovableEntity delegates to MovableComponent
    //        ✅ All entities are ICollidable (nullable Hitbox for non-collidables like ghosts)
    //
    //      ⏳ FUTURE WORK - When needed:
    //        - Add collision properties to ICollidable (Restitution, CollisionLayer, CollisionMask, etc.)
    //          Note: These are just properties, NOT complex logic, so NO CollisionComponent needed!
    //        - Add DamageComponent if we need complex damage calculation logic (health, armor math, etc.)
    //        - Add InventoryComponent if we need complex inventory management logic
    //
    //      Architecture principle:
    //        - Use COMPONENTS for interfaces with COMPLEX METHOD LOGIC (e.g., IMovable.Accelerate)
    //        - Use SIMPLE PROPERTIES for interfaces with just data (e.g., ICollidable.Hitbox)
    //        - This avoids the diamond problem without unnecessary abstraction

    public abstract class Entity : ICollidable
    {
        protected Entity()
        {

        }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Texture2D Sprite { get; set; }
        public abstract Hitbox Hitbox { get; set; }
        public abstract void Update(float deltaTime);
        public virtual void Draw(Camera2D camera)
        {
            var size = new Vector2(Sprite.Width / camera.Zoom, Sprite.Height / camera.Zoom);
            var origin = size / 2f;
            var source = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
            var dest = new Rectangle(Position, size);

            Raylib.DrawTexturePro(Sprite, source, dest, origin, Rotation, Color.White);
        }
    }
}
