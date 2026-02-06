using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public class StandardGridService : IGridService
    {
        public (GridEntity, RefillStep) Refill(GridEntity grid, int seed, List<TileTypeID> availableTypes)
        {
            var newGrid = grid.Clone();
            var rand = new System.Random(seed);
            var newTiles = new List<TileEntity>();

            for (int y = 0; y < newGrid.Height; y++)
            {
                for (int x = 0; x < newGrid.Width; x++)
                {
                    if (newGrid.GetTile(x, y).IsEmpty)
                    {
                        var type = availableTypes[rand.Next(availableTypes.Count)];
                        // Generate a unique ID. Ideally this should come from a generator or state, 
                        // but for deterministic simple refill we can hash coords + seed + step index or just random.
                        // For this implementation, let's use a large random range.
                        var id = rand.Next(10000, 999999); 
                        
                        var newTile = new TileEntity { TypeId = type, UniqueId = id };
                        newGrid.SetTile(x, y, newTile);
                        newTiles.Add(newTile);
                    }
                }
            }

            return (newGrid, new RefillStep { ResultingGrid = newGrid, NewTiles = newTiles });
        }
    }
}