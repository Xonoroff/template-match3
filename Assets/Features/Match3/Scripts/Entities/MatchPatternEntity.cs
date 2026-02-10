using System.Collections.Generic;
using Features.Match3.Scripts.Entities.Configs;

namespace Features.Match3.Scripts.Entities
{
    public record MatchPatternEntity
    {
        public readonly IReadOnlyList<TileCoordinateEntity> TileCoordinates;

        public MatchPatternEntity(IReadOnlyList<TileCoordinateEntity> tileCoordinates)
        {
            TileCoordinates = tileCoordinates;
        }
    }
}