using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public class StandardMatchService : IMatchService
    {
        public List<MatchPatternEntity> GetConnectedTiles(GridEntity grid, int startX, int startY, int minDistance = 2)
        {
            var startTile = grid.GetTile(startX, startY);
            if (startTile.IsEmpty)
            {
                return new List<MatchPatternEntity>();
            }

            var connectedCoordinates = new HashSet<TileCoordinateEntity>();
            FindConnectedTiles(grid, startX, startY, startTile.Type, connectedCoordinates);

            if (connectedCoordinates.Count >= minDistance)
            {
                return new List<MatchPatternEntity>
                {
                    new MatchPatternEntity { TileCoordinates = new List<TileCoordinateEntity>(connectedCoordinates) }
                };
            }

            return new List<MatchPatternEntity>();
        }

        private void FindConnectedTiles(GridEntity grid, int x, int y, TileTypeIDEntity targetType, HashSet<TileCoordinateEntity> visited)
        {
            if (x < 0 || x >= grid.Width || y < 0 || y >= grid.Height) return;

            var coordinate = new TileCoordinateEntity(x, y);
            if (visited.Contains(coordinate)) return;

            var tile = grid.GetTile(x, y);
            if (tile.IsEmpty || tile.Type != targetType) return;

            visited.Add(coordinate);

            FindConnectedTiles(grid, x + 1, y, targetType, visited);
            FindConnectedTiles(grid, x - 1, y, targetType, visited);
            FindConnectedTiles(grid, x, y + 1, targetType, visited);
            FindConnectedTiles(grid, x, y - 1, targetType, visited);
        }
    }
}