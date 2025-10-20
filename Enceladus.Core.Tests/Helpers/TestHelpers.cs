using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.World;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Tests.Helpers
{
    /// <summary>
    /// Common test helpers for map creation
    /// </summary>
    public static class MapHelpers
    {
        /// <summary>
        /// Creates a map with chunks at the specified coordinates
        /// </summary>
        public static Map CreateMapWithChunks(params (int x, int y)[] chunkCoords)
        {
            var map = new Map();
            foreach (var (x, y) in chunkCoords)
            {
                map.Chunks[(x, y)] = new MapChunk(x, y);
            }
            return map;
        }

        /// <summary>
        /// Creates a map with cells at the specified coordinates.
        /// Automatically determines and creates the necessary chunks based on cell positions.
        /// All cells are created as Ice (with collision).
        /// </summary>
        /// 
        //todo: make test fixture for these helpers maybe?
        public static Map CreateMapWithCells(params (int x, int y)[] cellCoords)
        {
            var map = new Map();

            // Group cells by their chunk coordinates
            var cellsByChunk = cellCoords
                .GroupBy(coord => ChunkMath.WorldToChunkCoords(coord.x, coord.y))
                .ToArray();

            foreach (var chunkGroup in cellsByChunk)
            {
                var (chunkX, chunkY) = chunkGroup.Key;
                var chunk = new MapChunk(chunkX, chunkY);

                foreach (var (x, y) in chunkGroup)
                {
                    chunk.Cells.Add(new Cell
                    {
                        X = x,
                        Y = y,
                        CellType = CellTypes.Ice // Ice has collision
                    });
                }

                map.Chunks[(chunkX, chunkY)] = chunk;
            }

            return map;
        }
    }

    /// <summary>
    /// Test helper entity for general entity testing.
    /// Use EntityHelpers.CreateTestEntity() to create instances.
    /// </summary>
    public class TestEntity : Entity
    {
        public TestEntity()
        {
            Guid = Guid.NewGuid();
            Hitbox = new RectHitbox(new(1,1)); // Default hitbox
        }

        public override IHitbox Hitbox { get; set; }
        public override void Update(float deltaTime) { }
    }

    /// <summary>
    /// Factory methods for creating test entities
    /// </summary>
    public static class EntityHelpers
    {
        /// <summary>
        /// Creates a test entity at the specified position
        /// </summary>
        public static TestEntity CreateTestEntity(Vector2 position, IHitbox? hitbox = null, float rotation = 0f)
        {
            hitbox ??= new RectHitbox(new(1, 1));
            return new TestEntity { 
                Position = position,
                Hitbox = hitbox,
                Rotation = rotation
            };
        }
    }
}
