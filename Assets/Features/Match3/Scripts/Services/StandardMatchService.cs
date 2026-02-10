using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public class StandardMatchService : IMatchService
    {
        public List<MatchPatternEntity> GetConnectedTiles(GridEntity grid, int startX, int startY)
        {
            var matches = new List<MatchPatternEntity>();

            var state = grid.Clone();
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
                CheckNeighbor(cx + 1, cy, state, startTile.Type, visited, queue);
                CheckNeighbor(cx - 1, cy, state, startTile.Type, visited, queue);
                CheckNeighbor(cx, cy + 1, state, startTile.Type, visited, queue);
                CheckNeighbor(cx, cy - 1, state, startTile.Type, visited, queue);
            }

            if (connectedIndices.Count >= 2)
            {
                var matchCoordinates = new List<TileCoordinateEntity>();
                foreach (var index in connectedIndices)
                {
                    matchCoordinates.Add(new TileCoordinateEntity(index % state.Width, index / state.Width));
                }

                matches.Add(new MatchPatternEntity
                {
                    TileCoordinates = matchCoordinates
                });
            }

            return matches;
        }

        private void CheckNeighbor(int x, int y, GridEntity grid, TileTypeIDEntity targetType, HashSet<int> visited, Queue<(int x, int y)> queue)
        {
            if (x < 0 || x >= grid.Width || y < 0 || y >= grid.Height) return;

            int index = y * grid.Width + x;
            if (visited.Contains(index)) return;

            var tile = grid.GetTile(x, y);
            if (!tile.IsEmpty && tile.Type == targetType)
            {
                visited.Add(index);
                queue.Enqueue((x, y));
            }
        }
    }
}