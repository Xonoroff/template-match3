using System;
using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct LevelConfigEntity
    {
        public int Width;
        public int Height;
        public TileTypeIDEntity[] AvailableColors;
        public int Seed;
        public string AtlasAddress;
        public string SpritePrefix;
    }
}