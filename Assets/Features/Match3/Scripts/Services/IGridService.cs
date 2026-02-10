using System.Collections.Generic;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;
using Features.Match3.Scripts.Entities.Steps;

namespace Features.Match3.Scripts.Services
{
    public interface IGridService
    {
        (GridStateEntity, RefillStepEntity) Refill(GridStateEntity gridState, int seed, IList<TileTypeIDEntity> availableTypes);
    }
}