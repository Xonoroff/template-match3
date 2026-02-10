using Features.Match3.Scripts.Entities;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;

namespace Features.Match3.Scripts.Services
{
    public interface IMatch3Evaluator
    {

        ResolveSequenceEntity ResolveTap(GridStateEntity gridState, int x, int y, LevelConfigEntity config);
    }
}