using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public class StandardGravityService : IGravityService
    {
        public (GridEntity, GravityStepEntity) ApplyGravity(GridEntity grid)
        {
            var newGrid = grid.Clone();
            var drops = new List<TilePlacementEntity>();

            for (int x = 0; x < newGrid.Width; x++)
            {
                int emptyCount = 0;
                for (int y = 0; y < newGrid.Height; y++)
                {
                    if (newGrid.GetTile(x, y).IsEmpty)
                    {
                        emptyCount++;
                    }
                    else if (emptyCount > 0)
                    {
                        var tile = newGrid.GetTile(x, y);
                        int targetY = y - emptyCount;

                        newGrid.SetTile(x, targetY, tile);
                        newGrid.SetTile(x, y, TileEntity.Empty);

                        drops.Add(new TilePlacementEntity
                        {
                            Tile = tile,
                            Coordinates = new TileCoordinateEntity(x, targetY)
                        });
                    }
                }
            }

            var step = new GravityStepEntity { ResultingGrid = newGrid, Items = drops };
            return (newGrid, step);
        }
    }
}