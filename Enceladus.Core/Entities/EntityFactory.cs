using Enceladus.Core.Config;
using Enceladus.Core.Entities.TestMonsters;
using Enceladus.Core.Input;
using Enceladus.Core.Physics.Hitboxes.Helpers;
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
        private readonly IPolygonHitboxBuilder _polygonHitboxBuilder;
        private readonly IControllableRegistry _controllableRegistry;

        public EntityFactory(IEntityRegistry entityRegistry, IConfigService configService, ISpriteService spriteService,
            IPolygonHitboxBuilder polygonHitboxBuilder, IControllableRegistry controllableRegistry)
        {
            _entityRegistry = entityRegistry;
            _configService = configService;
            _spriteService = spriteService;
            _polygonHitboxBuilder = polygonHitboxBuilder;
            _controllableRegistry = controllableRegistry;
        }

        /// <summary>
        /// Please run this early on inside the createXentity functions to apply default values from config
        /// this is another please dont forget bro function
        /// </summary>
        private void ApplyDefaults(Entity entity)
        {
            if (entity is MovableEntity movableEntity)
            {
                movableEntity.Mass = _configService.Config.Physics.DefaultMass;
                movableEntity.Drag = _configService.Config.Physics.DefaultDrag;
                movableEntity.AngularDrag = _configService.Config.Physics.DefaultAngularDrag;
            }
        }

        /// <summary>
        /// Registers entity with all appropriate registries.
        /// Call this at the END of CreateX methods after all setup is complete.
        /// just dont forget bro please trust me bro
        /// </summary>
        private void RegisterEntity(Entity entity)
        {
            if (entity is IControllable controllableEntity)
            {
                _controllableRegistry.Register(controllableEntity);
            }

            _entityRegistry.Register(entity);
        }

        public Player CreatePlayer(Vector2 position)
        {
            var player = new Player()
            {
            };
            ApplyDefaults(player);

            // Player-specific config values (override defaults)
            var config = _configService.Config.Player;
            player.Mass = config.Mass;
            player.MainEngineThrust = config.MainEngineThrust;
            player.ManeuveringThrust = config.ManeuveringThrust;
            player.ManeuveringRotationalAuthority = config.ManeuveringRotationalAuthority;
            player.ManeuveringDampingStrength = config.ManeuveringDampingStrength;
            player.ManeuveringFinsAuthority = config.ManeuveringFinsAuthority;
            player.BrakeStrength = config.BrakeStrength;
            player.MinVelocityForRotation = config.MinVelocityForRotation;
            player.MinVelocityForMainEngine = config.MinVelocityForMainEngine;
            player.MaxAlignmentErrorDegrees = config.MaxAlignmentErrorDegrees;

            player.Hitbox = _polygonHitboxBuilder.BuildFromPixelCoordinates(
                (int)SpriteDefinitions.Entities.PlayerSubRight.SourceRegion.Width,
                (int)SpriteDefinitions.Entities.PlayerSubRight.SourceRegion.Height,
                Player.PixelVertices);
            player.Position = position;
            RegisterEntity(player);
            return player;
        }

        public EvilBlueTriangle CreateEvilBlueTriangle(Vector2 position)
        {
            var entity = new EvilBlueTriangle();
            ApplyDefaults(entity);
            entity.Position = position;
            RegisterEntity(entity);
            return entity;
        }

        public HorribleYellowCircle CreateHorribleYellowCircle(Vector2 position)
        {
            var entity = new HorribleYellowCircle();
            ApplyDefaults(entity);
            entity.Position = position;
            RegisterEntity(entity);
            return entity;
        }

        public MenacingRedPentagon CreateMenacingRedPentagon(Vector2 position)
        {
            var entity = new MenacingRedPentagon();
            ApplyDefaults(entity);
            entity.Position = position;
            RegisterEntity(entity);
            return entity;
        }

        public AwfulGreenStar CreateAwfulGreenStar(Vector2 position)
        {
            var entity = new AwfulGreenStar();
            ApplyDefaults(entity);

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
            entity.Hitbox = _polygonHitboxBuilder.BuildFromVertices(vertices);
            entity.Position = position;
            RegisterEntity(entity);
            return entity;
        }
    }
}
