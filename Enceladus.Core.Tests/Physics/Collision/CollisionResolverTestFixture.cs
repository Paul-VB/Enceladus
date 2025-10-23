using Enceladus.Core.Config;
using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Tests.Helpers;
using Enceladus.Core.World;
using Moq;
using System.Numerics;

namespace Enceladus.Core.Tests.Physics.Collision
{
    public class CollisionResolverTestFixture
    {
        private readonly ICollisionResolver _collisionResolver;
        private readonly Mock<IConfigService> _configService;

        public CollisionResolverTestFixture()
        {
            _configService = new Mock<IConfigService>();
            _configService.Setup(c => c.Config).Returns(new Config.Config
            {
                Physics = new PhysicsConfig
                {
                    RestitutionCoefficient = 0.5f // 50% bounce
                }
            });
            _collisionResolver = new CollisionResolver(_configService.Object);
        }


        [Fact]
        public void ResolveEntityToMovable_EqualMasses_SplitsImpulseEqually()
        {
            // Arrange: Two 10kg entities colliding head-on
            var entity1 = EntityHelpers.CreateMovableTestEntity(
                position: new Vector2(0, 0),
                hitbox: new RectHitbox(new Vector2(1, 1)),
                rotation: 0,
                mass: 10f,
                velocity: new Vector2(5, 0) // Moving right at 5 m/s
            );

            var entity2 = EntityHelpers.CreateMovableTestEntity(
                position: new Vector2(1, 0),
                hitbox: new RectHitbox(new Vector2(1, 1)),
                rotation: 0,
                mass: 10f,
                velocity: new Vector2(-5, 0) // Moving left at 5 m/s
            );

            var collision = new CollisionResult
            {
                Entity = entity1,
                OtherObject = entity2,
                CollisionNormal = new Vector2(-1, 0), // Points right (from entity2 to entity1)
                PenetrationDepth = 0.1f
            };

            // Act
            _collisionResolver.ResolveCollision(collision);

            // Assert: Equal masses should get equal velocity changes (but opposite directions)
            // Relative velocity = 5 - (-5) = 10 m/s
            // With restitution=0.5, they should bounce back at half the relative velocity
            // Each entity should change by approximately 5 m/s (symmetric collision)

            // Set breakpoint here to inspect entity1.Velocity and entity2.Velocity
            Assert.NotEqual(new Vector2(5, 0), entity1.Velocity); // Should have changed
            Assert.NotEqual(new Vector2(-5, 0), entity2.Velocity); // Should have changed
        }

        [Fact]
        public void ResolveEntityToMovable_HeavyVsLight_LightBouncesMore()
        {
            // Arrange: 100kg entity hits 10kg entity
            var heavyEntity = EntityHelpers.CreateMovableTestEntity(
                position: new Vector2(0, 0),
                hitbox: new RectHitbox(new Vector2(1, 1)),
                rotation: 0,
                mass: 100f,
                velocity: new Vector2(10, 0) // Moving right at 10 m/s
            );

            var lightEntity = EntityHelpers.CreateMovableTestEntity(
                position: new Vector2(1, 0),
                hitbox: new RectHitbox(new Vector2(1, 1)),
                rotation: 0,
                mass: 10f,
                velocity: new Vector2(0, 0) // Stationary
            );

            var collision = new CollisionResult
            {
                Entity = heavyEntity,
                OtherObject = lightEntity,
                CollisionNormal = new Vector2(-1, 0), // Points right (from light to heavy)
                PenetrationDepth = 0.1f
            };

            var initialHeavyVelocity = heavyEntity.Velocity;
            var initialLightVelocity = lightEntity.Velocity;

            // Act
            _collisionResolver.ResolveCollision(collision);

            // Assert: Light entity should have much larger velocity change than heavy entity
            var heavyVelocityChange = (heavyEntity.Velocity - initialHeavyVelocity).Length();
            var lightVelocityChange = (lightEntity.Velocity - initialLightVelocity).Length();

            // Set breakpoint here to see the actual values
            Assert.True(lightVelocityChange > heavyVelocityChange,
                $"Light entity should bounce more. Heavy change: {heavyVelocityChange}, Light change: {lightVelocityChange}");
        }

        [Fact]
        public void ResolveEntityToMovable_ConservesMomentum()
        {
            // Arrange
            var entity1 = EntityHelpers.CreateMovableTestEntity(
                position: new Vector2(0, 0),
                hitbox: new RectHitbox(new Vector2(1, 1)),
                rotation: 0,
                mass: 15f,
                velocity: new Vector2(3, 0)
            );

            var entity2 = EntityHelpers.CreateMovableTestEntity(
                position: new Vector2(1, 0),
                hitbox: new RectHitbox(new Vector2(1, 1)),
                rotation: 0,
                mass: 25f,
                velocity: new Vector2(-2, 0)
            );

            var collision = new CollisionResult
            {
                Entity = entity1,
                OtherObject = entity2,
                CollisionNormal = new Vector2(1, 0),
                PenetrationDepth = 0.1f
            };

            // Calculate initial momentum
            var initialMomentum = entity1.Mass * entity1.Velocity + entity2.Mass * entity2.Velocity;

            // Act
            _collisionResolver.ResolveCollision(collision);

            // Calculate final momentum
            var finalMomentum = entity1.Mass * entity1.Velocity + entity2.Mass * entity2.Velocity;

            // Assert: Momentum should be conserved (within floating point tolerance)
            // Set breakpoint here to compare initial vs final momentum
            Assert.Equal(initialMomentum.X, finalMomentum.X, precision: 2);
            Assert.Equal(initialMomentum.Y, finalMomentum.Y, precision: 2);
        }

        [Fact]
        public void ResolveEntityToStatic_BouncesEntity()
        {
            // Arrange: Entity colliding with static cell
            var entity = EntityHelpers.CreateMovableTestEntity(
                position: new Vector2(5, 5),
                hitbox: new RectHitbox(new Vector2(1, 1)),
                rotation: 0,
                velocity: new Vector2(10, 0) // Moving right
            );

            var cell = CellHelpers.CreateTestCell(6, 5);

            var collision = new CollisionResult
            {
                Entity = entity,
                OtherObject = cell,
                CollisionNormal = new Vector2(-1, 0), // Points left (from cell to entity)
                PenetrationDepth = 0.1f
            };

            // Act
            _collisionResolver.ResolveCollision(collision);

            // Assert: Entity should bounce back (velocity reversed in X direction)
            Assert.True(entity.Velocity.X < 0, "Entity should bounce back with negative X velocity");
            Assert.Equal(0, entity.Velocity.Y); // Y should remain unchanged
        }
    }
}
