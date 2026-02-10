using Features.Match3.Scripts.Entities.Configs;
using UnityEngine;

namespace Features.Match3.Scripts.Views.Entities
{
    public struct TileViewEntity
    {
        public int UniqueId;
        public TileTypeIDEntity TypeId;
        public Sprite Sprite;
        public TileCoordinateEntity Coordinate;
        public bool IsEmpty => UniqueId == 0;
    }
}
