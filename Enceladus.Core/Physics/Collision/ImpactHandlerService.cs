namespace Enceladus.Core.Physics.Collision
{
    /// <summary>
    /// Handles game logic that occurs after collisions are resolved.
    /// This includes damage, effects, sounds, particle spawns, etc.
    /// Keeps collision detection/resolution separate from game logic.
    /// </summary>
    public interface IImpactHandlerService
    {
        /// <summary>
        /// Handles the impact between two colliding objects.
        /// Called after collision physics has been resolved.
        /// Routes to appropriate handlers based on collision types.
        /// </summary>
        void HandleImpact(CollisionResult collision);
    }

    public class ImpactHandlerService : IImpactHandlerService
    {
        //TODO: Implement impact handling logic
        //TODO: Route collision types to appropriate handlers:
        //  - Projectile vs Cell -> damage cell, destroy if health <= 0, call chunk.NotifyCellsChanged()
        //  - Projectile vs Entity -> damage entity
        //  - Player vs Cell -> handle bumping/ramming
        //  - Explosion vs anything -> area damage
        //TODO: Call chunk.NotifyCellsChanged() when cells are destroyed
        //TODO: Spawn particle effects, play sounds, apply knockback, etc.

        public void HandleImpact(CollisionResult collision)
        {
            // Stub - no impact handling yet
            // When implemented, this will route collisions to specific handlers
        }
    }
}
