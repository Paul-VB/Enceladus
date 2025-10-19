using Enceladus.Core.Config;
using Enceladus.Core.Entities.TestMonsters;
using Enceladus.Core.Input;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;
using System.Numerics;

namespace Enceladus.Core.Entities
{
    public interface IEntityFactory
    {
        Player CreatePlayer(Vector2 position);
        EvilBlueTriangle CreateEvilBlueTriangle(Vector2 position);
        HorribleYellowCircle CreateHorribleYellowCircle(Vector2 position);
        MenacingRedPentagon CreateMenacingRedPentagon(Vector2 position);
        AwfulGreenStar CreateAwfulGreenStar(Vector2 position);
    }

    public class EntityFactory : IEntityFactory
    {
        private readonly IEntityRegistry _entityRegistry;
        private readonly IConfigService _configService;
        private readonly ISpriteService _spriteService;
        private readonly IInputManager _inputManager;
        private readonly IPolygonHitboxBuilder _polygonHitboxBuilder;

        public EntityFactory(
            IEntityRegistry entityRegistry,
            IConfigService configService,
            ISpriteService spriteService,
            IInputManager inputManager,
            IPolygonHitboxBuilder polygonHitboxBuilder)
        {
            _entityRegistry = entityRegistry;
            _configService = configService;
            _spriteService = spriteService;
            _inputManager = inputManager;
            _polygonHitboxBuilder = polygonHitboxBuilder;
        }

        public Player CreatePlayer(Vector2 position)
        {
            var player = new Player(_inputManager, _spriteService, _configService);
            player.Hitbox = _polygonHitboxBuilder.BuildFromPixelCoordinates(
                player.Sprite.Width,
                player.Sprite.Height,
                Player.PixelVertices);
            player.Position = position;
            _entityRegistry.Register(player);
            return player;
        }

        public EvilBlueTriangle CreateEvilBlueTriangle(Vector2 position)
        {
            var entity = new EvilBlueTriangle(_inputManager, _configService);
            entity.Position = position;
            _entityRegistry.Register(entity);
            return entity;
        }

        public HorribleYellowCircle CreateHorribleYellowCircle(Vector2 position)
        {
            var entity = new HorribleYellowCircle(_inputManager, _configService);
            entity.Position = position;
            _entityRegistry.Register(entity);
            return entity;
        }

        public MenacingRedPentagon CreateMenacingRedPentagon(Vector2 position)
        {
            var entity = new MenacingRedPentagon(_configService);
            entity.Position = position;
            _entityRegistry.Register(entity);
            return entity;
        }

        public AwfulGreenStar CreateAwfulGreenStar(Vector2 position)
        {
            var entity = new AwfulGreenStar(_inputManager, _configService);

            // Create 5-pointed star vertices (4 units tall)
            var vertices = new List<Vector2>();
            float outerRadius = 2f;
            float innerRadius = 0.8f;

            for (int i = 0; i < 10; i++)
            {
                float angle = (i * 36f - 90f) * MathF.PI / 180f; // 36 degrees per point (360/10)
                float radius = i % 2 == 0 ? outerRadius : innerRadius;
                vertices.Add(new Vector2(radius * MathF.Cos(angle), radius * MathF.Sin(angle)));
            }

            // Build concave hitbox from star vertices
            entity.Hitbox = _polygonHitboxBuilder.BuildFromOuterVertices(vertices);
            entity.Position = position;
            _entityRegistry.Register(entity);
            return entity;
        }
    }
}
