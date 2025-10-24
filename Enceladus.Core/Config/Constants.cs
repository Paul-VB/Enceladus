namespace Enceladus.Core.Config
{
    public static class Constants
    {
        /// <summary>
        /// Number of sprite pixels per world unit. Defines the scale of the entire world.
        /// For example, a 16x16 pixel sprite = 1x1 world unit.
        /// </summary>
        public const float PixelsPerWorldUnit = 16f;

        /// <summary>
        /// Number of cells per chunk (width and height).
        /// Chunks are square regions used for spatial partitioning and efficient rendering.
        /// </summary>
        public const int ChunkSize = 16;
    }
}
