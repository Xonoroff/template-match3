using System;
using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{

    public class ResolveSequence
    {
        public GridEntity InitialState;
        public List<GameStep> Steps = new List<GameStep>();
    }
}
