using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public interface IGridService
    {
        GridEntity CreateGrid(int width, int height);
        void SetTile(ref GridEntity grid, TileCoordinateEntity coordinate, TileEntity tile);
        TileEntity GetTile(in GridEntity grid, TileCoordinateEntity coordinate);
        void SwapTiles(ref GridEntity grid, TileCoordinateEntity a, TileCoordinateEntity b);
        bool IsValid(in GridEntity grid, TileCoordinateEntity coordinate);
        List<TilePlacementEntity> GetRefillForColumn(in GridEntity grid, int x, IList<TileTypeIDEntity> availableColors);
        (GridEntity, RefillStepEntity) Refill(GridEntity grid, int seed, IList<TileTypeIDEntity> availableTypes);
    }
}