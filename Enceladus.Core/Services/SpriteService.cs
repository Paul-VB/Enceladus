using Raylib_cs;

namespace Enceladus.Core.Services
{
    public interface ISpriteService
    {
        Texture2D Load(string path);
        void UnloadAll();
    }
    public class SpriteService : ISpriteService
    {
        private Dictionary<string, Texture2D> _cache = new();

        public Texture2D Load(string path)
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

    public static class Sprites
    {
        public const string PlayerSubLeft = "assets/sprites/playerSub/playerSubLeft.png";
        public const string PlayerSubRight = "assets/sprites/playerSub/playerSubRight.png";

        public const string Water = "assets/sprites/cells/0-Water.png";
        public const string Ice = "assets/sprites/cells/1-Ice.png";
    }
}
