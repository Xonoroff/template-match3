using System.Collections.Generic;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Views;
using UnityEngine;

namespace Features.Match3.Scripts.Presenters.StepConverters
{
    public class MatchStepVisualConverter : IStepVisualConverter
    {
        public bool CanConvert(GameStepEntity step)
        {
            return step is MatchStepEntity;
        }

        public VisualStep Convert(GameStepEntity step, IReadOnlyDictionary<TileTypeIDEntity, Sprite> spriteMap)
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
