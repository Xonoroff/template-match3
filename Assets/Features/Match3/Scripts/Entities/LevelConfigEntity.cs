using System;
using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct LevelConfigEntity
    {
        public int Id;
        public int Width;
        public int Height;
        public List<TileTypeID> AvailableTileTypes;
        public int Seed;
        public string AtlasAddress;
        public string SpritePrefix;
    }
}