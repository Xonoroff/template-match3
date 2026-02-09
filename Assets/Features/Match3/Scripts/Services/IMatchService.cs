using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public interface IMatchService
    {
        List<MatchPatternEntity> GetConnectedTiles(GridEntity grid, int startX, int startY);
    }
}