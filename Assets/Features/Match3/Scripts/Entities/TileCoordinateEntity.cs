using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public struct TileCoordinateEntity : IEquatable<TileCoordinateEntity>
    {
        public int X;
        public int Y;

        public TileCoordinateEntity(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(TileCoordinateEntity other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is TileCoordinateEntity other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(TileCoordinateEntity left, TileCoordinateEntity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TileCoordinateEntity left, TileCoordinateEntity right)
        {
            return !left.Equals(right);
        }
    }
}
