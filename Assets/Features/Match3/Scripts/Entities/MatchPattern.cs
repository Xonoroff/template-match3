using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public struct MatchPattern
    {
        public TileTypeID TypeId;
        
        public List<int> TileIndices;
    }
}