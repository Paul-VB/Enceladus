using Enceladus.Core.Entities;

namespace Enceladus.Core
{
    public interface IEntityRegistry
    {
        IReadOnlyDictionary<Guid, Entity> Entities { get; }
        IReadOnlyList<MovableEntity> MovableEntities { get; }
        IReadOnlyList<Entity> StaticEntities { get; }
        T Register<T>(T entity) where T : Entity;
        void Unregister(Guid guid);
    }

    public class EntityRegistry : IEntityRegistry
    {
        private readonly Dictionary<Guid, Entity> _entities = new();
        private readonly List<MovableEntity> _movableEntities = new();
        private readonly List<Entity> _staticEntities = new();

        public IReadOnlyDictionary<Guid, Entity> Entities => _entities;
        public IReadOnlyList<MovableEntity> MovableEntities => _movableEntities;
        public IReadOnlyList<Entity> StaticEntities => _staticEntities;

        public T Register<T>(T entity) where T : Entity
        {
            _entities[entity.Guid] = entity;

            if (entity is MovableEntity moveable)
                _movableEntities.Add(moveable);
            else
                _staticEntities.Add(entity);

            return entity;
        }

        public void Unregister(Guid guid)
        {
            if (_entities.TryGetValue(guid, out var entity))
            {
                _entities.Remove(guid);

                if (entity is MovableEntity moveable)
                    _movableEntities.Remove(moveable);
                else
                    _staticEntities.Remove(entity);
            }
        }
    }
}
