using System.Numerics;

namespace Enceladus.Core.World.Chunks
{
    public interface IIslandOutlineTracer
    {
        /// <summary>
        /// Traces the outline of an island using marching squares algorithm.
        /// Returns vertices of the polygon outline in counter-clockwise order.
        /// </summary>
        List<Vector2> TraceOutline(Island island);
    }

    public class IslandOutlineTracer : IIslandOutlineTracer
    {
        internal enum Direction
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }

        public List<Vector2> TraceOutline(Island island)
        {
            var vertices = new List<Vector2>();

            if (island.Count == 0)
                return vertices;

            // Find a starting edge cell (every island must have at least one edge)



            var startCell = FindEdgeCell(island);


            // Start walking from the edge cell
            var currentDirection = FindInitialDirectionCounterClockwise2(island, startCell);
            var startDirection = currentDirection;

            // Walk the perimeter until we return to start
            var currentCell = startCell;
            var visited = new HashSet<((int, int) cell, Direction dir)>();

            var addVector = (int x, int y, string dir) =>
            {
                var v = new Vector2(x, y);
                Console.Write($"attempting to add back-{dir} corner vertex ({x}, {y}). Vertex #{vertices.Count}. currentDirection: {currentDirection}. currentCell: {currentCell}... ");
                if (!vertices.Contains(v))
                {
                    vertices.Add(new Vector2(x, y));
                    Console.WriteLine("success!");
                } else
                {
                    Console.WriteLine( "fail, that vertex is already in there");
                }
            };

            do
            {
                // Mark this state as visited before processing
                var state = (currentCell, currentDirection);
                visited.Add(state);

                //Console.WriteLine($"At cell {currentCell} facing {currentDirection}");
                //look right
                if (HasRightNeighbor(island, currentDirection, currentCell))
                {
                    Console.WriteLine("  -> Turning RIGHT");
                    currentDirection = TurnRight(currentDirection);
                    var (x, y) = GetFrontRightCornerVertex(currentDirection, currentCell);
                    addVector(x, y, "left");
                }
   
                //look ahead
                else if (HasForwardNeighbor(island, currentDirection, currentCell))
                {
                    Console.WriteLine("  -> Going FORWARD (no vertex)");
                    //keep looking forward. im pretty sure this seemingly empty if statement MUST be here.
                }
                //look left
                else if (HasLeftNeighbor(island, currentDirection, currentCell))
                {
                    Console.WriteLine("  -> Turning LEFT");
                    currentDirection = TurnLeft(currentDirection);
                    var (x, y) = GetBackRightCornerVertex(currentDirection, currentCell);
                    addVector(x, y, "right");
                }
                //look behind
                else
                {
                    Console.WriteLine("  -> Turning AROUND (adding 2 vertices)");
                    //we are at a dead end. a 1 wide "wallway" of solid blocks. turn around and mark 2 vertecies
                    currentDirection = TurnAround(currentDirection);
                    var (xl, yl) = GetBackLeftCornerVertex(currentDirection, currentCell);
                    addVector(xl, yl, "left");
                    var (xr, yr) = GetBackRightCornerVertex(currentDirection, currentCell);
                    addVector(xr, yr, "right");

                    var InSingleCellIsland = (!HasForwardNeighbor(island, currentDirection, currentCell));
                    if (InSingleCellIsland)
                    {
                        // Already added back-left and back-right after first turn-around
                        // Turn around again to get the other two corners (completes the 360° loop)
                        currentDirection = TurnAround(currentDirection);
                        var (xl2, yl2) = GetBackLeftCornerVertex(currentDirection, currentCell);
                        addVector(xl2, yl2, "left");
                        var (xr2, yr2) = GetBackRightCornerVertex(currentDirection, currentCell);
                        addVector(xr2, yr2, "right");
                        break;
                    }
                }
                currentCell = StepInDirection(currentDirection, currentCell);

                // Check if we've been in this exact state before
                if (visited.Contains((currentCell, currentDirection)))
                    break;  // We've looped back - done!

            } while (true);

            return vertices;
        }

        private (int x, int y) FindEdgeCell(Island island)
        {
            foreach (var cell in island)
            {
                var hasNorthNeighbor = HasNorthNeighbor(island, cell);
                var hasEastNeighbor = HasEastNeighbor(island, cell);
                var hasSouthNeighbor = HasSouthNeighbor(island, cell);
                var hasWestNeighbor = HasWestNeighbor(island, cell);

                // If not all 4 neighbors exist, this is an edge cell
                if (!(hasNorthNeighbor && hasEastNeighbor && hasSouthNeighbor && hasWestNeighbor))
                    return cell;
            }

            throw new InvalidOperationException("Island has no edge cells - this should be impossible!");
        }

        private Direction FindInitialDirectionCounterClockwise2(Island island, (int x, int y) startCell)
        {
            // Start by facing a direction where:
            // - Empty space is to our RIGHT (no neighbor in that direction)
            // - Solid is AHEAD or to our LEFT (to keep island on left as we walk)

            // Try each direction and pick the first one with empty space to the right
            if (!HasEastNeighbor(island, startCell))  // Empty to East
                return Direction.North;  // Face North, empty is to our right

            if (!HasNorthNeighbor(island, startCell))  // Empty to north
                return Direction.West;  // Face west, empty is to our right

            if (!HasWestNeighbor(island, startCell))  // Empty to West
                return Direction.South;  // Face South, empty is to our right

            // Empty to North
            return Direction.East;  // Face West, empty is to our right
        }

        internal bool HasNorthNeighbor(Island island, (int x, int y) cell) => island.Contains((cell.x, cell.y + 1));
        internal bool HasEastNeighbor(Island island, (int x, int y) cell) => island.Contains((cell.x + 1, cell.y));
        internal bool HasSouthNeighbor(Island island, (int x, int y) cell) => island.Contains((cell.x, cell.y - 1));
        internal bool HasWestNeighbor(Island island, (int x, int y) cell) => island.Contains((cell.x - 1, cell.y));

        private (int x, int y) StepInDirection(Direction direction, (int x, int y) currentCell)
        {
            switch (direction)
            {
                case Direction.North:
                    return (currentCell.x, currentCell.y + 1);
                case Direction.East:
                    return (currentCell.x + 1, currentCell.y);
                case Direction.South:
                    return (currentCell.x, currentCell.y - 1);
                default: //west
                    return (currentCell.x - 1, currentCell.y);
            }
        }

        private bool HasLeftNeighbor(Island island, Direction currentDirection, (int x, int y) currentCell)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    return HasWestNeighbor(island, currentCell);
                case Direction.East:
                    return HasNorthNeighbor(island, currentCell);
                case Direction.South:
                    return HasEastNeighbor(island, currentCell);
                default: //west
                    return HasSouthNeighbor(island, currentCell);
            }
        }

        private bool HasForwardNeighbor(Island island, Direction currentDirection, (int x, int y) currentCell)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    return HasNorthNeighbor(island, currentCell);
                case Direction.East:
                    return HasEastNeighbor(island, currentCell);
                case Direction.South:
                    return HasSouthNeighbor(island, currentCell);
                default: //west
                    return HasWestNeighbor(island, currentCell);
            }
        }

        private bool HasRightNeighbor(Island island, Direction currentDirection, (int x, int y) currentCell)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    return HasEastNeighbor(island, currentCell);
                case Direction.East:
                    return HasSouthNeighbor(island, currentCell);
                case Direction.South:
                    return HasWestNeighbor(island, currentCell);
                default: //west
                    return HasNorthNeighbor(island, currentCell);
            }
        }

        private Direction TurnLeft(Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    return Direction.West;
                case Direction.East:
                    return Direction.North;
                case Direction.South:
                    return Direction.East;
                default: //west
                    return Direction.South;
            }
        }

        private Direction TurnRight(Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    return Direction.East;
                case Direction.East:
                    return Direction.South;
                case Direction.South:
                    return Direction.West;
                default: //west
                    return Direction.North;
            }
        }

        private Direction TurnAround(Direction currentDirection)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                default: //west
                    return Direction.East;
            }
        }

        internal (int x, int y) GetBackLeftCornerVertex(Direction currentDirection, (int x, int y) currentCell)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    return (currentCell.x, currentCell.y);
                case Direction.East:
                    return (currentCell.x, currentCell.y + 1);
                case Direction.South:
                    return (currentCell.x + 1, currentCell.y + 1);
                default: //west
                    return (currentCell.x + 1, currentCell.y);
            }
        }

        internal (int x, int y) GetBackRightCornerVertex(Direction currentDirection, (int x, int y) currentCell)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    return (currentCell.x + 1, currentCell.y);
                case Direction.East:
                    return (currentCell.x, currentCell.y);
                case Direction.South:
                    return (currentCell.x, currentCell.y + 1);
                default: //west
                    return (currentCell.x + 1, currentCell.y + 1);
            }
        }

        internal (int x, int y) GetFrontRightCornerVertex(Direction currentDirection, (int x, int y) currentCell)
        {
            switch (currentDirection)
            {
                case Direction.North:
                    return (currentCell.x + 1, currentCell.y + 1);
                case Direction.East:
                    return (currentCell.x + 1, currentCell.y);
                case Direction.South:
                    return (currentCell.x, currentCell.y);
                default: //west
                    return (currentCell.x, currentCell.y + 1);
            }
        }
    }

}
