using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Rendering;
using Enceladus.Core.Entities.Weapons;
using Enceladus.Core.Entities.Projectiles;
using Enceladus.Core.MotionControl.PlayerMotion;
using Enceladus.Core.MotionControl.AIMotion;

namespace Enceladus.Core.Entities
{
    public interface IEntityRegistry
    {
        Player? Player { get; }
        IReadOnlyDictionary<Guid, Entity> Entities { get; }
        IReadOnlyList<MovableEntity> MovableEntities { get; }
        IReadOnlyList<ICollidable> StaticCollidables { get; }
        IReadOnlyList<ISpriteRendered> SpriteRenderedEntities { get; }
        IReadOnlyList<IGeometryRendered> GeometryRenderedEntities { get; }
        IReadOnlyList<IArmed> ArmedEntities { get; }
        IReadOnlyList<Projectile> Projectiles { get; }
        IReadOnlyList<IPlayerMovable> PlayerMovableEntities { get; }
        IReadOnlyList<IAIMovable> AIMovableEntities { get; }
        T Register<T>(T entity) where T : Entity;
        void Unregister(Guid guid);
    }

    public class EntityRegistry : IEntityRegistry
    {
        private readonly Dictionary<Guid, Entity> _entities = new();
        private readonly List<MovableEntity> _movableEntities = new();
        private readonly List<ICollidable> _staticCollidables = new();
        private readonly List<ISpriteRendered> _spriteRenderedEntities = new();
        private readonly List<IGeometryRendered> _geometryRenderedEntities = new();
        private readonly List<IArmed> _armedEntities = new();
        private readonly List<Projectile> _projectiles = new();
        private readonly List<IPlayerMovable> _playerMovableEntities = new();
        private readonly List<IAIMovable> _aiMovableEntities = new();
        private Player? _player;

        public Player? Player => _player;
        public IReadOnlyDictionary<Guid, Entity> Entities => _entities;
        public IReadOnlyList<MovableEntity> MovableEntities => _movableEntities;
        public IReadOnlyList<ICollidable> StaticCollidables => _staticCollidables;
        public IReadOnlyList<ISpriteRendered> SpriteRenderedEntities => _spriteRenderedEntities;
        public IReadOnlyList<IGeometryRendered> GeometryRenderedEntities => _geometryRenderedEntities;
        public IReadOnlyList<IArmed> ArmedEntities => _armedEntities;
        public IReadOnlyList<Projectile> Projectiles => _projectiles;
        public IReadOnlyList<IPlayerMovable> PlayerMovableEntities => _playerMovableEntities;
        public IReadOnlyList<IAIMovable> AIMovableEntities => _aiMovableEntities;

        public T Register<T>(T entity) where T : Entity
        {
            _entities[entity.Guid] = entity;

            if (entity is Player player)
                _player = player;

            if (entity is MovableEntity moveable)
                _movableEntities.Add(moveable);
            else if (entity is ICollidable collidable)
                _staticCollidables.Add(collidable);

            if (entity is ISpriteRendered spriteRendered)
                _spriteRenderedEntities.Add(spriteRendered);
            else if (entity is IGeometryRendered geometryRendered)
                _geometryRenderedEntities.Add(geometryRendered);

            if (entity is IArmed armed)
                _armedEntities.Add(armed);

            if (entity is Projectile projectile)
                _projectiles.Add(projectile);

            if (entity is IPlayerMovable playerMovable)
                _playerMovableEntities.Add(playerMovable);

            if (entity is IAIMovable aiMovable)
                _aiMovableEntities.Add(aiMovable);

            return entity;
        }

        public void Unregister(Guid guid)
        {
            if (!_entities.TryGetValue(guid, out var entity))
                return;

            _entities.Remove(guid);

            if (entity is Player)
                _player = null;

            if (entity is MovableEntity moveable)
                _movableEntities.Remove(moveable);
            if (entity is ICollidable collidable && entity is not MovableEntity)
                _staticCollidables.Remove(collidable);
            if (entity is ISpriteRendered spriteRendered)
                _spriteRenderedEntities.Remove(spriteRendered);
            if (entity is IGeometryRendered geometryRendered)
                _geometryRenderedEntities.Remove(geometryRendered);
            if (entity is IArmed armed)
                _armedEntities.Remove(armed);
            if (entity is Projectile projectile)
                _projectiles.Remove(projectile);
            if (entity is IPlayerMovable playerMovable)
                _playerMovableEntities.Remove(playerMovable);
            if (entity is IAIMovable aiMovable)
                _aiMovableEntities.Remove(aiMovable);
        }
    }
}
