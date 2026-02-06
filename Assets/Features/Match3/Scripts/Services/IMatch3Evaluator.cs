namespace Features.Match3.Scripts.Domain
{
    public interface IMatch3Evaluator
    {
        /// <summary>
        /// Resolves a move into a full sequence of steps (Swap -> Match -> Gravity -> Match -> ... -> Stable).
        /// </summary>
        ResolveSequence ResolveTap(GridEntity startState, int x, int y, LevelConfigEntity config);
    }
}