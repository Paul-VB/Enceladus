using Raylib_cs;

namespace Enceladus.Core.Rendering
{
    public interface ISpriteService
    {
        Texture2D GetTextureAtlas(string path);
        void UnloadAll();
    }

    public class SpriteService : ISpriteService
    {
        private Dictionary<string, Texture2D> _cache = new();

        public Texture2D GetTextureAtlas(string path)
        {
            if (!_cache.ContainsKey(path))
                _cache[path] = Raylib.LoadTexture(path);

            return _cache[path];
        }

        public void UnloadAll()
        {
            foreach (var texture in _cache.Values)
            {
                Raylib.UnloadTexture(texture);
            }
            _cache.Clear();
        }
    }
}
