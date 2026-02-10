using System.Collections.Generic;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;
using Features.Match3.Scripts.Entities.Steps;

namespace Features.Match3.Scripts.Services
{
    public class StandardGravityService : IGravityService
    {
        public (GridStateEntity, GravityStepEntity) ApplyGravity(GridStateEntity gridState)
        {
            var drops = new List<TileEntity>();

            for (int x = 0; x < gridState.Width; x++)
            {
                int emptyCount = 0;
                for (int y = 0; y < gridState.Height; y++)
                {
                    if (gridState.GetTile(x, y).IsEmpty)
                    {
                        emptyCount++;
                    }
                    else if (emptyCount > 0)
                    {
                        var tile = gridState.GetTile(x, y);
                        int targetY = y - emptyCount;

                        var newCoordinate = new TileCoordinateEntity(x, targetY);
                        var movedTile = tile.CloneWith(newCoordinate);

                        gridState.SetTile(x, targetY, movedTile);
                        gridState.SetTile(x, y, TileEntity.Empty(new TileCoordinateEntity(x, y)));

                        drops.Add(movedTile);
                    }
                }
            }

            var step = new GravityStepEntity(gridState, drops);
            return (gridState, step);
        }
    }
}