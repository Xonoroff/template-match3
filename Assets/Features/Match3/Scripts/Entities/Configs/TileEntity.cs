using System;

namespace Features.Match3.Scripts.Entities.Configs
{
    [Serializable]
    public readonly struct TileEntity
    {
        private const int EmptyId = -1;
        
        public readonly int UniqueId;
        public readonly TileTypeIDEntity Type;
        public readonly TileCoordinateEntity Coordinate;

        public TileEntity(int uniqueId, TileTypeIDEntity type, TileCoordinateEntity coordinate)
        {
            Type = type;
            UniqueId = uniqueId;
            Coordinate = coordinate;
        }

        public bool IsEmpty => UniqueId == EmptyId;

        public static TileEntity Empty(TileCoordinateEntity coordinate) => new TileEntity (EmptyId, TileTypeIDEntity.None, coordinate );
        
        public TileEntity CloneWith(TileCoordinateEntity coordinate) => new TileEntity(UniqueId, Type, coordinate);
    }
}
