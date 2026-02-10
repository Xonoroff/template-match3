using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct GridEntity
    {
        public int Width;
        public int Height;
        public TileEntity[] Tiles; //TODO: breaks immutability

        public GridEntity(int width, int height)
        {
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

            var placedTile = tile;
            placedTile.Coordinate = new TileCoordinateEntity(x, y);
            Tiles[y * Width + x] = placedTile;
        }

        public GridEntity Clone()
        {
            var newGrid = new GridEntity(Width, Height);
            Array.Copy(Tiles, newGrid.Tiles, Tiles.Length);
            return newGrid;
        }
    }
}