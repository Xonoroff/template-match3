namespace Features.Match3.Scripts.Domain
{
    public interface IMatch3Evaluator
    {
        ResolveSequence ResolveTap(GridEntity startState, int x, int y, LevelConfigEntity config);
    }
}