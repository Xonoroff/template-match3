using System.Collections.Generic;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;
using Features.Match3.Scripts.Entities.Steps;

namespace Features.Match3.Scripts.Services
{
    public class StandardGridService : IGridService
    {

        public (GridStateEntity, RefillStepEntity) Refill(GridStateEntity gridState, int seed, IList<TileTypeIDEntity> availableTypes)
        {
            var rand = new System.Random(seed);
            var newTiles = new List<TileEntity>();

            for (int y = 0; y < gridState.Height; y++)
            {
                for (int x = 0; x < gridState.Width; x++)
                {
                    if (gridState.GetTile(x, y).IsEmpty)
                    {
                        var type = availableTypes[rand.Next(availableTypes.Count)];
                        var id = rand.Next(10000, 999999);

                        var newCoordinate = new TileCoordinateEntity(x, y);
                        var newTile = new TileEntity(id, type, newCoordinate);
                        
                        gridState.SetTile(x, y, newTile);
                        
                        newTiles.Add(newTile);
                    }
                }
            }

            return (gridState, new RefillStepEntity(gridState, newTiles));
        }
    }
}