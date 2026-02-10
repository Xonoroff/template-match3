using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public interface IGridService
    {
        (GridEntity, RefillStepEntity) Refill(GridEntity grid, int seed, IList<TileTypeIDEntity> availableTypes);
    }
}