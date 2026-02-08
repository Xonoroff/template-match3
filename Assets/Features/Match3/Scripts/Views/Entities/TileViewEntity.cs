using Features.Match3.Scripts.Domain;
using UnityEngine;

namespace Features.Match3.Scripts.Views
{
    public struct TileViewEntity
    {
        public int UniqueId;
        public TileTypeID TypeId;
        public Sprite Sprite;
        public bool IsEmpty => UniqueId == 0;
    }
}
