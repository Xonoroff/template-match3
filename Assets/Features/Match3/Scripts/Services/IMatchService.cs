using System.Collections.Generic;
using Features.Match3.Scripts.Entities;
using Features.Match3.Scripts.Entities.States;

namespace Features.Match3.Scripts.Services
{
    public interface IMatchService
    {
        List<MatchPatternEntity> GetConnectedTiles(GridStateEntity gridState, int startX, int startY, int minDistance = 2);
    }
}