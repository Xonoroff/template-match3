using System.Collections.Generic;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Views;
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

            foreach (var newTileData in refillStep.Items)
            {
                _spriteMap.TryGetValue(newTileData.Tile.Type, out var sprite);

                visualStep.Actions.Add(new SpawnVisualAction
                {
                    X = newTileData.Tile.Coordinate.X,
                    Y = newTileData.Tile.Coordinate.Y,
                    Tile = new TileViewEntity
                    {
                        UniqueId = newTileData.Tile.UniqueId,
                        TypeId = newTileData.Tile.Type,
                        Sprite = sprite,
                        Coordinate = newTileData.Tile.Coordinate
                    }
                });
            }

            return visualStep;
        }
    }
}
