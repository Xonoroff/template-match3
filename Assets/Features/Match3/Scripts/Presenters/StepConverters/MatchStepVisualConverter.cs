using Features.Match3.Scripts.Entities.Steps;
using Features.Match3.Scripts.Views;
using Features.Match3.Scripts.Views.Actions;

namespace Features.Match3.Scripts.Presenters.StepConverters
{
    public class MatchStepVisualConverter : IStepVisualConverter
    {

        public bool CanConvert(GameStepEntity step)
        {
            return step is MatchStepEntity;
        }

        public VisualStep Convert(GameStepEntity step)
        {
            var matchStep = (MatchStepEntity)step;
            var visualStep = new VisualStep();

            foreach (var pattern in matchStep.Matches)
            {
                foreach (var coord in pattern.TileCoordinates)
                {
                    visualStep.Actions.Add(new DestroyVisualAction
                    {
                        X = coord.X,
                        Y = coord.Y
                    });
                }
            }

            return visualStep;
        }
    }
}
