using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public class StandardGridService : IGridService
    {

        public (GridEntity, RefillStepEntity) Refill(GridEntity grid, int seed, IList<TileTypeIDEntity> availableTypes)
        {
            var newGrid = grid.Clone();
            var rand = new System.Random(seed);
            var newTiles = new List<TilePlacementEntity>();

            for (int y = 0; y < newGrid.Height; y++)
            {
                for (int x = 0; x < newGrid.Width; x++)
                {
                    if (newGrid.GetTile(x, y).IsEmpty)
                    {
                        var type = availableTypes[rand.Next(availableTypes.Count)];
                        var id = rand.Next(10000, 999999);

                        var newTile = new TileEntity(type, new TileCoordinateEntity(x, y)) { UniqueId = id };
                        newGrid.SetTile(x, y, newTile);
                        newTiles.Add(new TilePlacementEntity
                        {
                            Tile = newTile
                        });
                    }
                }
            }

            return (newGrid, new RefillStepEntity { ResultingGrid = newGrid, Items = newTiles });
        }
    }
}