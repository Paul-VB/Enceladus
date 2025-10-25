namespace Enceladus.Core.World.Chunks
{
    //todo: should this be a struct? im still fuzzy on the performance benefits of structs vs classes
    public class MapChunk
    {
        public int X { get; }
        public int Y { get; }

        public List<Cell> Cells { get; set; } = [];
        public List<Physics.Hitboxes.ConcavePolygonHitbox> MergedHitboxes { get; set; } = [];

        // Event fired when cells are added, removed, or modified
        public event Action? OnCellsChanged;

        public MapChunk(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Notifies subscribers that cells have been modified.
        /// Call this after adding, removing, or modifying cells.
        /// </summary>
        public void NotifyCellsChanged()
        {
            OnCellsChanged?.Invoke();
        }
    }
}
