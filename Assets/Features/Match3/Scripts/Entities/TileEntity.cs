using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct TileEntity
    {
        public TileTypeIDEntity Type;
        public int UniqueId;
        public TileCoordinateEntity Coordinate;

        public TileEntity(TileTypeIDEntity type, TileCoordinateEntity coordinate)
        {
            Type = type;
            UniqueId = 0;
            Coordinate = coordinate;
        }

        public bool IsEmpty => UniqueId == 0;

        public static TileEntity Empty(TileCoordinateEntity coordinate) => new TileEntity { UniqueId = 0, Type = TileTypeIDEntity.None, Coordinate = coordinate };
    }
}
