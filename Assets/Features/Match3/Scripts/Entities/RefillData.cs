using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct RefillData
    {
        public TileCoordinate Coordinates;
        public TileEntity Tile;
    }
}
