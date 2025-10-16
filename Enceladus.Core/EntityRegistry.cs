using Enceladus.Core.Entities;

namespace Enceladus.Core
{
    //todo: can we mess with the base Entity class to make it register itself whevener it gets created? maybe we need an entityFactory?
    public interface IEntityRegistry
    {
        public Dictionary<Guid, IEntity> Entities { get; }
        T Register<T>(T entity) where T : IEntity;
    }
    public class EntityRegistry : IEntityRegistry
    {
        public Dictionary<Guid, IEntity> Entities { get; } = new Dictionary<Guid, IEntity>();

        public T Register<T>(T entity) where T : IEntity
        {
            Entities[entity.Guid] = entity;
            return entity;
        }
    }
}
