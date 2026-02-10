using Features.Match3.Scripts.Entities.States;
using Features.Match3.Scripts.Entities.Steps;

namespace Features.Match3.Scripts.Services
{
    public interface IGravityService
    {
        (GridStateEntity, GravityStepEntity) ApplyGravity(GridStateEntity gridState);
    }
}