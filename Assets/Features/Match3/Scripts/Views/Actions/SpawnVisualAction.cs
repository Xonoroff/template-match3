using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Views
{
    public struct SpawnVisualAction : IVisualAction
    {
        public int X, Y;
        public TileViewEntity Tile;
    }
}