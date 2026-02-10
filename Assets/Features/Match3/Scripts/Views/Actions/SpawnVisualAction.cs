using Features.Match3.Scripts.Views.Entities;

namespace Features.Match3.Scripts.Views.Actions
{
    public struct SpawnVisualAction : IVisualAction
    {
        public int FromX, FromY;
        public TileViewEntity Tile;
    }
}