using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public interface IGridService
    {
        /// <summary>
        /// Fills empty tiles based on deterministic logic.
        /// </summary>
        (GridEntity, RefillStep) Refill(GridEntity grid, int seed, List<TileTypeID> availableTypes);
    }
}