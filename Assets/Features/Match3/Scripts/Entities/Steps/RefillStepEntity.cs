using System.Collections.Generic;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;

namespace Features.Match3.Scripts.Entities.Steps
{
    public record RefillStepEntity : GameStepEntity
    {
        public List<TileEntity> Items { get; private set; }

        public RefillStepEntity(GridStateEntity resultingGridState, List<TileEntity> items) : base(resultingGridState)
        {
            Items = items;
        }
    }
}