using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Views
{
    public struct TileViewEntity
    {
        public int UniqueId;
        public TileTypeID TypeId;
        public bool IsEmpty => UniqueId == 0;
    }
}
