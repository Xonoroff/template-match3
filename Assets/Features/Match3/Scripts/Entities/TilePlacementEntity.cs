using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct TilePlacementEntity
    {
        public TileCoordinateEntity Coordinates;
        public TileEntity Tile;
    }
}
