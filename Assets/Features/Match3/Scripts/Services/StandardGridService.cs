using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public class StandardGridService : IGridService
    {
        public GridEntity CreateGrid(int width, int height)
        {
            return new GridEntity(width, height);
        }

        public void SetTile(ref GridEntity grid, TileCoordinateEntity coordinate, TileEntity tile)
        {
            grid.SetTile(coordinate.X, coordinate.Y, tile);
        }

        public TileEntity GetTile(in GridEntity grid, TileCoordinateEntity coordinate)
        {
            return grid.GetTile(coordinate.X, coordinate.Y);
        }

        public void SwapTiles(ref GridEntity grid, TileCoordinateEntity a, TileCoordinateEntity b)
        {
            var tileA = grid.GetTile(a.X, a.Y);
            var tileB = grid.GetTile(b.X, b.Y);
            grid.SetTile(a.X, a.Y, tileB);
            grid.SetTile(b.X, b.Y, tileA);
        }

        public bool IsValid(in GridEntity grid, TileCoordinateEntity coordinate)
        {
            return coordinate.X >= 0 && coordinate.X < grid.Width && coordinate.Y >= 0 && coordinate.Y < grid.Height;
        }

        public List<TilePlacementEntity> GetRefillForColumn(in GridEntity grid, int x, IList<TileTypeIDEntity> availableColors)
        {
            var results = new List<TilePlacementEntity>();
            var rand = new System.Random();

            for (int y = 0; y < grid.Height; y++)
            {
                if (grid.GetTile(x, y).IsEmpty)
                {
                    var type = availableColors[rand.Next(availableColors.Count)];
                    results.Add(new TilePlacementEntity
                    {
                        Coordinates = new TileCoordinateEntity(x, y),
                        Tile = new TileEntity { Type = type, UniqueId = rand.Next() }
                    });
                }
            }
            return results;
        }

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

                        var newTile = new TileEntity { Type = type, UniqueId = id };
                        newGrid.SetTile(x, y, newTile);
                        newTiles.Add(new TilePlacementEntity
                        {
                            Coordinates = new TileCoordinateEntity(x, y),
                            Tile = newTile
                        });
                    }
                }
            }

            return (newGrid, new RefillStepEntity { ResultingGrid = newGrid, Items = newTiles });
        }
    }
}