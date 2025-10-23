using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities
{
    public interface IEntityRegistry
    {
        IReadOnlyDictionary<Guid, Entity> Entities { get; }
        IReadOnlyList<MovableEntity> MovableEntities { get; }
        IReadOnlyList<ICollidable> StaticCollidables { get; }
        IReadOnlyList<ISpriteRendered> SpriteRenderedEntities { get; }
        IReadOnlyList<IGeometryRendered> GeometryRenderedEntities { get; }
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

        public IReadOnlyDictionary<Guid, Entity> Entities => _entities;
        public IReadOnlyList<MovableEntity> MovableEntities => _movableEntities;
        public IReadOnlyList<ICollidable> StaticCollidables => _staticCollidables;
        public IReadOnlyList<ISpriteRendered> SpriteRenderedEntities => _spriteRenderedEntities;
        public IReadOnlyList<IGeometryRendered> GeometryRenderedEntities => _geometryRenderedEntities;

        public T Register<T>(T entity) where T : Entity
        {
            _entities[entity.Guid] = entity;

            if (entity is MovableEntity moveable)
                _movableEntities.Add(moveable);
            else if (entity is ICollidable collidable)
                _staticCollidables.Add(collidable);

            if (entity is ISpriteRendered spriteRendered)
                _spriteRenderedEntities.Add(spriteRendered);
            else if (entity is IGeometryRendered geometryRendered)
                _geometryRenderedEntities.Add(geometryRendered);

            return entity;
        }

        public void Unregister(Guid guid)
        {
            if (!_entities.TryGetValue(guid, out var entity))
                return;

            _entities.Remove(guid);

            if (entity is MovableEntity moveable)
                _movableEntities.Remove(moveable);
            if (entity is ICollidable collidable && entity is not MovableEntity)
                _staticCollidables.Remove(collidable);
            if (entity is ISpriteRendered spriteRendered)
                _spriteRenderedEntities.Remove(spriteRendered);
            if (entity is IGeometryRendered geometryRendered)
                _geometryRenderedEntities.Remove(geometryRendered);
        }
    }
}
