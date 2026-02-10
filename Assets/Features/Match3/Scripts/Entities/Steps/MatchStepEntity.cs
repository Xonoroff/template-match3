using System.Collections.Generic;
using Features.Match3.Scripts.Entities.States;

namespace Features.Match3.Scripts.Entities.Steps
{
    public record MatchStepEntity : GameStepEntity
    {
        public List<MatchPatternEntity> Matches { get; private set; }
        
        public MatchStepEntity(GridStateEntity resultingGridState, List<MatchPatternEntity> matches) : base(resultingGridState)
        {
            Matches = matches;
        }
    }
}