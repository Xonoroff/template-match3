using System;

namespace Features.Match3.Scripts.Domain
{
    [Serializable]
    public readonly struct TileTypeIDEntity : IEquatable<TileTypeIDEntity>
    {
        public readonly int Value;

        public static readonly TileTypeIDEntity None = new TileTypeIDEntity(0);

        public TileTypeIDEntity(int value)
        {
            Value = value;
        }

        public bool Equals(TileTypeIDEntity other) => Value == other.Value;
        public override bool Equals(object obj) => obj is TileTypeIDEntity other && Equals(other);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(TileTypeIDEntity left, TileTypeIDEntity right) => left.Equals(right);
        public static bool operator !=(TileTypeIDEntity left, TileTypeIDEntity right) => !left.Equals(right);
        public override string ToString() => Value.ToString();
    }
}