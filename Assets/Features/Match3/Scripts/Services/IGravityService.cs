namespace Features.Match3.Scripts.Domain
{
    public interface IGravityService
    {
        /// <summary>
        /// Applies gravity to the grid. 
        /// Returns the new grid state and specific drop metadata for animation.
        /// </summary>
        (GridEntity, GravityStep) ApplyGravity(GridEntity grid);
    }
}