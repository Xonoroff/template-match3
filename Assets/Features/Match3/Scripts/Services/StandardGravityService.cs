using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public class StandardGravityService : IGravityService
    {
        public (GridEntity, GravityStep) ApplyGravity(GridEntity grid)
        {
            var newGrid = grid.Clone();
            var drops = new List<DropData>();

            // For each column
            for (int x = 0; x < newGrid.Width; x++)
            {
                int emptyCount = 0;
                // Scan from bottom to top
                for (int y = 0; y < newGrid.Height; y++)
                {
                    if (newGrid.GetTile(x, y).IsEmpty)
                    {
                        emptyCount++;
                    }
                    else if (emptyCount > 0)
                    {
                        // Stick tile down
                        var tile = newGrid.GetTile(x, y);
                        int targetY = y - emptyCount;

                        newGrid.SetTile(x, targetY, tile);
                        newGrid.SetTile(x, y, TileEntity.Empty);

                        // Record drop
                        drops.Add(new DropData
                        {
                            Tile = tile,
                            From = new TileCoordinate(x, y),
                            To = new TileCoordinate(x, targetY)
                        });
                    }
                }
            }

            var step = new GravityStep { ResultingGrid = newGrid, Drops = drops };
            return (newGrid, step);
        }
    }
}