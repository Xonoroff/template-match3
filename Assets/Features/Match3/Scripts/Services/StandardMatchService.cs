using System.Collections.Generic;
using Features.Match3.Scripts.Entities;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;

namespace Features.Match3.Scripts.Services
{
    public class StandardMatchService : IMatchService
    {
        public List<MatchPatternEntity> GetConnectedTiles(GridStateEntity gridState, int startX, int startY, int minDistance = 2)
        {
            var startTile = gridState.GetTile(startX, startY);
            if (startTile.IsEmpty)
            {
                return new List<MatchPatternEntity>();
            }

            var connectedCoordinates = new HashSet<TileCoordinateEntity>();
            FindConnectedTiles(gridState, startX, startY, startTile.Type, connectedCoordinates);

            if (connectedCoordinates.Count >= minDistance)
            {
                var tileCoordinates = new List<TileCoordinateEntity>(connectedCoordinates);
                return new List<MatchPatternEntity>
                {
                    new MatchPatternEntity(tileCoordinates)
                };
            }

            return new List<MatchPatternEntity>();
        }

        private void FindConnectedTiles(GridStateEntity gridState, int x, int y, TileTypeIDEntity targetType, HashSet<TileCoordinateEntity> visited)
        {
            if (x < 0 || x >= gridState.Width || y < 0 || y >= gridState.Height) return;

            var coordinate = new TileCoordinateEntity(x, y);
            if (visited.Contains(coordinate)) return;

            var tile = gridState.GetTile(x, y);
            if (tile.IsEmpty || tile.Type != targetType) return;

            visited.Add(coordinate);

            FindConnectedTiles(gridState, x + 1, y, targetType, visited);
            FindConnectedTiles(gridState, x - 1, y, targetType, visited);
            FindConnectedTiles(gridState, x, y + 1, targetType, visited);
            FindConnectedTiles(gridState, x, y - 1, targetType, visited);
        }
    }
}