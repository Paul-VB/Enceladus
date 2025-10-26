using Enceladus.Core.Config;
using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Tests.Helpers;
using Moq;
using System.Numerics;

namespace Enceladus.Core.Tests.Physics.Collision
{
    public class CollisionResolverTestFixture
    {
        private readonly ICollisionResolver _collisionResolver;
        private readonly Mock<IConfigService> _configService;


        private CollisionResult _testCollision;

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

        private MovableTestEntity GivenAMovableEntity(Vector2 position, float mass, Vector2 velocity)
        {
            var entity =  new MovableTestEntity
            {
                Position = position,
                Mass = mass,
                Velocity = velocity,
                Hitbox = new RectHitbox(new Vector2(1, 1)),
                Rotation = 0,
                Drag = 0.9f
            };
            return entity;
        }

        private void GivenACollision(MovableEntity entity, ICollidable otherObject, Vector2 collisionNormal)
        {
            var collision =  new CollisionResult
            {
                Entity = entity,
                OtherObject = otherObject,
                CollisionNormal = collisionNormal,
                PenetrationDepth = 0.1f
            };
            _testCollision = collision;
        }

        private void ResolveCollision()
        {
            _collisionResolver.ResolveCollision(_testCollision);
        }

        [Fact]
        public void GivenTwoEqualMassEntitiesCollidingHeadOn_WhenResolving_ThenBothVelocitiesChange()
        {
            // Given: Two 10kg entities colliding head-on
            var entity1 = GivenAMovableEntity(
                position: new Vector2(0, 0),
                mass: 10f,
                velocity: new Vector2(5, 0) // Moving right at 5 m/s
            );

            var entity2 = GivenAMovableEntity(
                position: new Vector2(1, 0),
                mass: 10f,
                velocity: new Vector2(-5, 0) // Moving left at 5 m/s
            );

            GivenACollision(entity1, entity2, new Vector2(-1, 0));

            // When
            ResolveCollision();

            // Then: Equal masses should get equal velocity changes (but opposite directions)
            Assert.NotEqual(new Vector2(5, 0), entity1.Velocity);
            Assert.NotEqual(new Vector2(-5, 0), entity2.Velocity);
        }

        [Fact]
        public void GivenHeavyEntityHitsLightEntity_WhenResolving_ThenLightEntityBouncesMore()
        {
            // Given: 100kg entity hits 10kg entity
            var heavyEntity = GivenAMovableEntity(
                position: new Vector2(0, 0),
                mass: 100f,
                velocity: new Vector2(10, 0) // Moving right at 10 m/s
            );

            var lightEntity = GivenAMovableEntity(
                position: new Vector2(1, 0),
                mass: 10f,
                velocity: new Vector2(0, 0) // Stationary
            );

            GivenACollision(heavyEntity, lightEntity, new Vector2(-1, 0));

            var initialHeavyVelocity = heavyEntity.Velocity;
            var initialLightVelocity = lightEntity.Velocity;

            // When
            ResolveCollision();

            // Then: Light entity should have much larger velocity change than heavy entity
            var heavyVelocityChange = (heavyEntity.Velocity - initialHeavyVelocity).Length();
            var lightVelocityChange = (lightEntity.Velocity - initialLightVelocity).Length();

            Assert.True(lightVelocityChange > heavyVelocityChange,
                $"Light entity should bounce more. Heavy change: {heavyVelocityChange}, Light change: {lightVelocityChange}");
        }

        [Fact]
        public void GivenTwoEntitiesColliding_WhenResolving_ThenMomentumIsConserved()
        {
            // Given
            var entity1 = GivenAMovableEntity(
                position: new Vector2(0, 0),
                mass: 15f,
                velocity: new Vector2(3, 0)
            );

            var entity2 = GivenAMovableEntity(
                position: new Vector2(1, 0),
                mass: 25f,
                velocity: new Vector2(-2, 0)
            );

            GivenACollision(entity1, entity2, new Vector2(1, 0));

            var initialMomentum = entity1.Mass * entity1.Velocity + entity2.Mass * entity2.Velocity;

            // When
            ResolveCollision();

            // Then: Momentum should be conserved (within floating point tolerance)
            var finalMomentum = entity1.Mass * entity1.Velocity + entity2.Mass * entity2.Velocity;

            Assert.Equal(initialMomentum.X, finalMomentum.X, precision: 2);
            Assert.Equal(initialMomentum.Y, finalMomentum.Y, precision: 2);
        }

        [Fact]
        public void GivenEntityCollidingWithStaticCell_WhenResolving_ThenEntityBouncesBack()
        {
            // Given: Entity colliding with static cell
            var entity = GivenAMovableEntity(
                position: new Vector2(5, 5),
                mass: 100f,
                velocity: new Vector2(10, 0) // Moving right
            );

            var cell = CellHelpers.GivenACell(6, 5);

            GivenACollision(entity, cell, new Vector2(-1, 0)); // Points left (from cell to entity)

            // When
            ResolveCollision();

            // Then: Entity should bounce back (velocity reversed in X direction)
            Assert.True(entity.Velocity.X < 0, "Entity should bounce back with negative X velocity");
            Assert.Equal(0, entity.Velocity.Y); // Y should remain unchanged
        }
    }
}
