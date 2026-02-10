using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Views
{
    public struct MoveVisualAction : IVisualAction
    {
        public int ToX, ToY;
        public TileViewEntity Tile;
    }
}