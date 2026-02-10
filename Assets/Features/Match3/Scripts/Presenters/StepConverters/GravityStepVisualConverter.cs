using System.Collections.Generic;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Views;
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
                int toX = drop.Tile.Coordinate.X;
                int toY = drop.Tile.Coordinate.Y;

                visualStep.Actions.Add(new MoveVisualAction
                {
                    ToX = toX,
                    ToY = toY,
                    Tile = new TileViewEntity
                    {
                        UniqueId = drop.Tile.UniqueId,
                        TypeId = drop.Tile.Type,
                        Coordinate = drop.Tile.Coordinate
                    }
                });
            }

            return visualStep;
        }
    }
}
