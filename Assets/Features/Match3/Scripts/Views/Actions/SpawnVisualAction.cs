using Features.Match3.Scripts.Views.Entities;

namespace Features.Match3.Scripts.Views.Actions
{
    public struct SpawnVisualAction : IVisualAction
    {
        public int X, Y;
        public TileViewEntity Tile;
    }
}