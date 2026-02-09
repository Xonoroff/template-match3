namespace Features.Match3.Scripts.Domain
{
    public interface IGravityService
    {
        (GridEntity, GravityStepEntity) ApplyGravity(GridEntity grid);
    }
}