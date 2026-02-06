namespace Features.Match3.Scripts.Domain
{
    public struct DropData
    {
        public int FromIndex; // -1 if new spawn
        public int ToIndex;
        public TileEntity Tile;
    }
}