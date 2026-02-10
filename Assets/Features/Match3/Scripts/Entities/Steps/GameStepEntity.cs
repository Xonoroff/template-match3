using Features.Match3.Scripts.Entities.States;

namespace Features.Match3.Scripts.Entities.Steps
{
    public abstract record GameStepEntity
    {
        public GridStateEntity ResultingGridState { get; private set; }

        protected GameStepEntity(GridStateEntity resultingGridState)
        {
            ResultingGridState = resultingGridState;
        }
    }
}