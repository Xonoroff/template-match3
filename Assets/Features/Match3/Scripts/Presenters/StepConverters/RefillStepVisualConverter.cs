using System.Collections.Generic;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.Steps;
using Features.Match3.Scripts.Views;
using Features.Match3.Scripts.Views.Actions;
using Features.Match3.Scripts.Views.Entities;
using UnityEngine;

namespace Features.Match3.Scripts.Presenters.StepConverters
{
    public class RefillStepVisualConverter : IStepVisualConverter
    {
        private IReadOnlyDictionary<TileTypeIDEntity, Sprite> _spriteMap;

        public RefillStepVisualConverter(IReadOnlyDictionary<TileTypeIDEntity, Sprite> spriteMap)
        {
            _spriteMap = spriteMap;
        }

        public bool CanConvert(GameStepEntity step)
        {
            return step is RefillStepEntity;
        }

        public VisualStep Convert(GameStepEntity step)
        {
            var refillStep = (RefillStepEntity)step;
            var visualStep = new VisualStep();
            const int offset = 2;
            foreach (var newTileData in refillStep.Items)
            {
                _spriteMap.TryGetValue(newTileData.Type, out var sprite);

                visualStep.Actions.Add(new SpawnVisualAction
                {
                    FromX = newTileData.Coordinate.X,
                    FromY = refillStep.ResultingGridState.Height + offset,
                    Tile = new TileViewEntity
                    {
                        UniqueId = newTileData.UniqueId,
                        TypeId = newTileData.Type,
                        Sprite = sprite,
                        Coordinate = newTileData.Coordinate
                    }
                });
            }

            return visualStep;
        }
    }
}
