using System;

namespace Features.Match3.Scripts.Entities.Configs
{
    [Serializable]
    public record LevelConfigEntity
    {
        public readonly int Width;
        public readonly int Height;
        public readonly int Seed;
        public readonly string AtlasAddress;
        public readonly string SpritePrefix;
        public readonly TileTypeIDEntity[] AvailableTileTypes;

        public LevelConfigEntity(int width, int height, TileTypeIDEntity[] availableTileTypes, int seed, string atlasAddress, string spritePrefix)
        {
            Width = width;
            Height = height;
            AvailableTileTypes = availableTileTypes;
            Seed = seed;
            AtlasAddress = atlasAddress;
            SpritePrefix = spritePrefix;
        }
    }
}