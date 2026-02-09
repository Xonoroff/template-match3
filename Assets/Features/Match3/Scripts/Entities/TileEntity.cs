using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct TileEntity
    {
        public TileTypeIDEntity Type;
        public int UniqueId;

        public TileEntity(TileTypeIDEntity type)
        {
            Type = type;
            UniqueId = 0;
        }

        public bool IsEmpty => UniqueId == 0;

        public static TileEntity Empty => new TileEntity { UniqueId = 0, Type = TileTypeIDEntity.None };
    }
}
