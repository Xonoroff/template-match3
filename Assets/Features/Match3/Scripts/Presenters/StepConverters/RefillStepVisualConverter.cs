using System.Collections.Generic;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Views;
using UnityEngine;

namespace Features.Match3.Scripts.Presenters.StepConverters
{
    public class RefillStepVisualConverter : IStepVisualConverter
    {
        public bool CanConvert(GameStepEntity step)
        {
            return step is RefillStepEntity;
        }

        public VisualStep Convert(GameStepEntity step, IReadOnlyDictionary<TileTypeIDEntity, Sprite> spriteMap)
        {
            var refillStep = (RefillStepEntity)step;
            var visualStep = new VisualStep();

            foreach (var newTileData in refillStep.Items)
            {
                spriteMap.TryGetValue(newTileData.Tile.Type, out var sprite);

                visualStep.Actions.Add(new SpawnVisualAction
                {
                    X = newTileData.Coordinates.X,
                    Y = newTileData.Coordinates.Y,
                    Tile = new TileViewEntity
                    {
                        UniqueId = newTileData.Tile.UniqueId,
                        TypeId = newTileData.Tile.Type,
                        Sprite = sprite
                    }
                });
            }

            return visualStep;
        }
    }
}
