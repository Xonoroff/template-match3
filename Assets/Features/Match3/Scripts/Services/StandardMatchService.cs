using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public class StandardMatchService : IMatchService
    {
        public List<MatchPattern> GetConnectedTiles(GridEntity grid, int startX, int startY)
        {
            var matches = new List<MatchPattern>();
            
            var state = grid; // In this architecture grid is stateless snapshot
            var startTile = state.GetTile(startX, startY);

            if (startTile.IsEmpty)
            {
                return matches;
            }

            var visited = new HashSet<int>();
            var connectedIndices = new List<int>();
            var queue = new Queue<(int x, int y)>();
            
            queue.Enqueue((startX, startY));
            visited.Add(startY * state.Width + startX);

            while (queue.Count > 0)
            {
                var (cx, cy) = queue.Dequeue();
                connectedIndices.Add(cy * state.Width + cx);

                // Check Neighbors
                CheckNeighbor(cx + 1, cy, state, startTile.TypeId, visited, queue);
                CheckNeighbor(cx - 1, cy, state, startTile.TypeId, visited, queue);
                CheckNeighbor(cx, cy + 1, state, startTile.TypeId, visited, queue);
                CheckNeighbor(cx, cy - 1, state, startTile.TypeId, visited, queue);
            }

            if (connectedIndices.Count >= 2) // Typically Match3 requires 3, but Blast might be 2+. Let's assume 2+ for tap.
            {
                 matches.Add(new MatchPattern 
                 { 
                     TileIndices = connectedIndices 
                 });
            }

            return matches;
        }

        private void CheckNeighbor(int x, int y, GridEntity grid, TileTypeID targetType, HashSet<int> visited, Queue<(int x, int y)> queue)
        {
            if (x < 0 || x >= grid.Width || y < 0 || y >= grid.Height) return;

            int index = y * grid.Width + x;
            if (visited.Contains(index)) return;

            var tile = grid.GetTile(x, y);
            if (!tile.IsEmpty && tile.TypeId == targetType)
            {
                visited.Add(index);
                queue.Enqueue((x, y));
            }
        }
    }
}