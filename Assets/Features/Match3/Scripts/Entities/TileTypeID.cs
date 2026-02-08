using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public readonly struct TileTypeID : IEquatable<TileTypeID>
    {
        public readonly int Value;

        public static readonly TileTypeID None = new TileTypeID(0);

        public TileTypeID(int value)
        {
            Value = value;
        }

        public bool Equals(TileTypeID other) => Value == other.Value;
        public override bool Equals(object obj) => obj is TileTypeID other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(TileTypeID left, TileTypeID right) => left.Equals(right);
        public static bool operator !=(TileTypeID left, TileTypeID right) => !left.Equals(right);
        public override string ToString() => Value.ToString();
    }
}