using Enceladus.Core.Entities;
using Raylib_cs;

namespace Enceladus.Core.Rendering
{
    public interface IEntityRenderer
    {
        void Draw(IEnumerable<IEntity> entities, Camera2D camera);
    }

    public class EntityRenderer : IEntityRenderer
    {
        public void Draw(IEnumerable<IEntity> entities, Camera2D camera)
        {
            foreach (var entity in entities)
            {
                entity.Draw(camera);
            }
        }
    }
}
