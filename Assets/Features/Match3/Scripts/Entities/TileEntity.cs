using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct TileEntity
    {
        public TileTypeID TypeId;
        public int UniqueId;
        public bool IsEmpty => UniqueId == 0;

        public static TileEntity Empty => new TileEntity { UniqueId = 0, TypeId = TileTypeID.None };
    }
}
