using Features.Match3.Scripts.Views.Entities;

namespace Features.Match3.Scripts.Views.Actions
{
    public struct MoveVisualAction : IVisualAction
    {
        public int ToX, ToY;
        public TileViewEntity Tile;
    }
}