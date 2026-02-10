using System.Collections.Generic;
using Features.Match3.Scripts.Entities.Steps;
using Features.Match3.Scripts.Views;
using Features.Match3.Scripts.Views.Actions;
using Features.Match3.Scripts.Views.Entities;
using UnityEngine;

namespace Features.Match3.Scripts.Presenters.StepConverters
{
    public class GravityStepVisualConverter : IStepVisualConverter
    {
        public bool CanConvert(GameStepEntity step)
        {
            return step is GravityStepEntity;
        }

        public VisualStep Convert(GameStepEntity step)
        {
            var gravityStep = (GravityStepEntity)step;
            var visualStep = new VisualStep();

            foreach (var drop in gravityStep.Items)
            {
                int toX = drop.Coordinate.X;
                int toY = drop.Coordinate.Y;

                visualStep.Actions.Add(new MoveVisualAction
                {
                    ToX = toX,
                    ToY = toY,
                    Tile = new TileViewEntity
                    {
                        UniqueId = drop.UniqueId,
                        TypeId = drop.Type,
                        Coordinate = drop.Coordinate
                    }
                });
            }

            return visualStep;
        }
    }
}
