using Enceladus.Core.Config;
using Enceladus.Core.Entities.TestMonsters;
using Enceladus.Core.Entities.Weapons;
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
        TestGun CreateTestGun(IArmed owner);
        IProjectile CreateProjectile(Weapon weapon);
    }

    public class EntityFactory : IEntityFactory
    {
        private readonly IEntityRegistry _entityRegistry;
        private readonly IConfigService _configService;
        private readonly ISpriteService _spriteService;
        private readonly IPolygonHitboxBuilder _polygonHitboxBuilder;
        private readonly IControllableRegistry _controllableRegistry;
        private readonly IPlayerFactory _playerFactory;
        private readonly IProjectileFactory _projectileFactory;

        public EntityFactory(IEntityRegistry entityRegistry, IConfigService configService, ISpriteService spriteService,
            IPolygonHitboxBuilder polygonHitboxBuilder, IControllableRegistry controllableRegistry, IPlayerFactory playerFactory, IProjectileFactory projectileFactory)
        {
            _entityRegistry = entityRegistry;
            _configService = configService;
            _spriteService = spriteService;
            _polygonHitboxBuilder = polygonHitboxBuilder;
            _controllableRegistry = controllableRegistry;
            _playerFactory = playerFactory;
            _projectileFactory = projectileFactory;
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
            var player = _playerFactory.CreatePlayer(position);
            var testGun = CreateTestGun(player);
            player.WeaponMounts[0].EquippedWeapon = testGun;
            RegisterEntity(player);
            return player;
        }

        public EvilBlueTriangle CreateEvilBlueTriangle(Vector2 position)
        {
            var entity = new EvilBlueTriangle();
            entity.Position = position;
            RegisterEntity(entity);
            return entity;
        }

        public HorribleYellowCircle CreateHorribleYellowCircle(Vector2 position)
        {
            var entity = new HorribleYellowCircle();
            entity.Position = position;
            RegisterEntity(entity);
            return entity;
        }

        public MenacingRedPentagon CreateMenacingRedPentagon(Vector2 position)
        {
            var entity = new MenacingRedPentagon();
            entity.Position = position;
            RegisterEntity(entity);
            return entity;
        }

        public AwfulGreenStar CreateAwfulGreenStar(Vector2 position)
        {
            var entity = new AwfulGreenStar();

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

        public TestGun CreateTestGun(IArmed owner)
        {
            var weapon = new TestGun { Owner = owner };
            RegisterEntity(weapon);
            return weapon;
        }

        public IProjectile CreateProjectile(Weapon weapon)
        {
            var projectile = _projectileFactory.CreateProjectile(weapon);
            RegisterEntity((Entity)projectile);
            return projectile;
        }
    }
}
