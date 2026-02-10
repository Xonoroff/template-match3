using System;
using Features.Match3.Scripts.Entities.Configs;

namespace Features.Match3.Scripts.Entities.States
{
    [Serializable]
    public class GridStateEntity
    {
        public readonly int ConfigId;
        public readonly int Width;
        public readonly int Height;
        public readonly TileEntity[] Tiles;

        public GridStateEntity(int id, int width, int height)
        {
            ConfigId = id;
            Width = width;
            Height = height;
            Tiles = new TileEntity[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Tiles[y * width + x] = TileEntity.Empty(new TileCoordinateEntity(x, y));
                }
            }
        }

        public TileEntity GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return TileEntity.Empty(new TileCoordinateEntity(x, y));
            return Tiles[y * Width + x];
        }

        public void SetTile(int x, int y, TileEntity tile)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;

            var coordinate = new TileCoordinateEntity(x, y);
            var newTile = new TileEntity(tile.UniqueId, tile.Type, coordinate);
            Tiles[y * Width + x] = newTile;
        }
    }
}