using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Collision.Detection;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Tests.Helpers;
using Enceladus.Core.World;
using Moq;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Tests.Physics.Collision
{
    public class AabbCollisionDetectorTestFixture
    {
        private readonly IAabbCollisionDetector _aabbCollisionDetector;
        private readonly Mock<IAabbCalculator> _aabbCalculator;

        public AabbCollisionDetectorTestFixture()
        {
            _aabbCalculator = new Mock<IAabbCalculator>();
            _aabbCollisionDetector = new AabbCollisionDetector(_aabbCalculator.Object);
        }

        private void GivenEntityHasCalculatedAabb(Entity entity, Rectangle aabb)
        {
            _aabbCalculator
                .Setup(calc => calc.CalculateAabb(entity))
                .Returns(aabb);
        }

        [Fact]
        public void CheckPotentialCellCollisions_EntityOverlapsMultipleCells_ReturnsOnlyCellsWithCollision()
        {
            // Arrange
            var entity = CreateTestEntity();
            var map = MapHelpers.CreateMapWithCells((0, 0), (1, 1));

            // Mock AABB calculator to return a rectangle covering cells (0,0), (1,0), (0,1), (1,1)
            GivenEntityHasCalculatedAabb(entity, new Rectangle(0, 0, 2, 2));

            // Act
            var result = _aabbCollisionDetector.CheckPotentialCellCollisions(entity, map);

            // Assert - Only ice cells (with collision) should be returned
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
            Assert.Contains(result, c => c.X == 1 && c.Y == 1);
            Assert.DoesNotContain(result, c => c.CellType == CellTypes.Water); // Water has no collision
        }

        [Fact]
        public void CheckPotentialCellCollisions_EntityOverlapsNoChunks_ReturnsEmpty()
        {
            // Arrange
            var entity = CreateTestEntity();
            var map = new Map();
            GivenEntityHasCalculatedAabb(entity, new Rectangle(1000, 1000, 10, 10));

            // Act
            var result = _aabbCollisionDetector.CheckPotentialCellCollisions(entity, map);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CheckPotentialCellCollisions_NegativeCoordinates_WorksCorrectly()
        {
            // Arrange
            var entity = CreateTestEntity();
            var map = MapHelpers.CreateMapWithCells((0, 0), (-1, -1));
            GivenEntityHasCalculatedAabb(entity, new Rectangle(-1, -1, 1.5f, 1.5f));

            // Act
            var result = _aabbCollisionDetector.CheckPotentialCellCollisions(entity, map);

            // Assert - Should find cells with collision in negative coords
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.X == -1 && c.Y == -1);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
        }

        [Fact]
        public void CheckPotentialCollision_EntitiesOverlap_ReturnsTrue()
        {
            // Arrange
            var entity1 = CreateTestEntity();
            var entity2 = CreateTestEntity();
            GivenEntityHasCalculatedAabb(entity1, new Rectangle(0, 0, 10, 10));
            GivenEntityHasCalculatedAabb(entity2, new Rectangle(5, 5, 10, 10));

            // Act
            var result = _aabbCollisionDetector.CheckPotentialCollision(entity1, entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckPotentialCollision_EntitiesDontOverlap_ReturnsFalse()
        {
            // Arrange
            var entity1 = CreateTestEntity();
            var entity2 = CreateTestEntity();
            GivenEntityHasCalculatedAabb(entity1, new Rectangle(0, 0, 10, 10));
            GivenEntityHasCalculatedAabb(entity2, new Rectangle(20, 20, 10, 10));

            // Act
            var result = _aabbCollisionDetector.CheckPotentialCollision(entity1, entity2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CheckPotentialCollision_EntitiesEdgeTouching_ReturnsFalse()
        {
            // Arrange
            var entity1 = CreateTestEntity();
            var entity2 = CreateTestEntity();
            GivenEntityHasCalculatedAabb(entity1, new Rectangle(0, 0, 10, 10));
            GivenEntityHasCalculatedAabb(entity2, new Rectangle(10, 0, 10, 10));

            // Act
            var result = _aabbCollisionDetector.CheckPotentialCollision(entity1, entity2);

            // Assert - Edge touching doesn't count as overlap
            Assert.False(result);
        }

        // Helper methods
        private StaticTestEntity CreateTestEntity()
        {
            return EntityHelpers.CreateStaticTestEntity(Vector2.Zero, new CircleHitbox(5f));
        }
    }
}
