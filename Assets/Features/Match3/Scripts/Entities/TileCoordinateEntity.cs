using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct TileCoordinateEntity
    {
        public int X;
        public int Y;

        public TileCoordinateEntity(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
