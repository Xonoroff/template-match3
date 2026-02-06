using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public interface IMatchService
    {
        /// <summary>
        /// Finds all matches in the current grid.
        /// </summary>
        // Returns list of connected tiles of same color (including start tile)
        List<MatchPattern> GetConnectedTiles(GridEntity grid, int x, int y);
    }
}