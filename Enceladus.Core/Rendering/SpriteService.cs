using Raylib_cs;

namespace Enceladus.Core.Rendering
{
    public interface ISpriteService
    {
        Texture2D Load(string path);
        void UnloadAll();
    }

    //todo: stretch goal. i have since learned that having one big texture image with all the textures is more efficient. maybe we refactor this?
    //such that the static class of sprites isnt a list of string paths to the pngs... but a list of vector2 pairs to tell where in the singular big texture image to grab the section we need for a given entity or cell?
    //however, this seems slightly annoying from a DX standpoint. is there a way we can keep separate pngs inside the assets, then at compile time stitch them all together automatically?
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
