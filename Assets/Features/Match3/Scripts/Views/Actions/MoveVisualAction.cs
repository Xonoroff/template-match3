using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Views
{
    public class MoveVisualAction : VisualAction
    {
        public int FromX, FromY; // If -1, -1, it's a spawn (conceptually) but usually we separate spawn
        public int ToX, ToY;
        public TileEntity Tile; // To ensure view has data
    }
}