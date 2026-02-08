using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct TileCoordinate
    {
        public int X;
        public int Y;

        public TileCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
