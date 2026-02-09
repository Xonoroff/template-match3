using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public interface IMatch3Evaluator
    {

        ResolveSequenceEntity ResolveTap(GridEntity grid, int x, int y, LevelConfigEntity config);
    }
}