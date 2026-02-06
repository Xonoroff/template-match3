using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct GridEntity
    {
        public int Width;
        public int Height;
        public TileEntity[] Tiles; // Flattened array for easier serialization/copying

        public GridEntity(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new TileEntity[width * height];
        }

        public TileEntity GetTile(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return TileEntity.Empty;
            return Tiles[y * Width + x];
        }

        public void SetTile(int x, int y, TileEntity tile)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;
            Tiles[y * Width + x] = tile;
        }

        public GridEntity Clone()
        {
            var newGrid = new GridEntity(Width, Height);
            Array.Copy(Tiles, newGrid.Tiles, Tiles.Length);
            return newGrid;
        }
    }
}