using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;
using Features.Match3.Scripts.Services;

namespace Features.Match3.Scripts.API
{
    public class MockMatch3API : IMatch3API
    {

        // Reuse the evaluator logic to simulate "server-side" calculation
        private readonly IMatch3Evaluator _evaluator;
        private readonly IGridService _gridService;

        private int _cachedSeed;
        private LevelConfigEntity _config;
        private GridStateEntity _serverState;

        public MockMatch3API(IMatch3Evaluator evaluator, IGridService gridService)
        {
            _evaluator = evaluator;
            _gridService = gridService;
        }

        //TODO: Is can also send back invalid code. E.g. user is cheater.
        public UniTask<(LevelConfigEntity config, GridStateEntity state)> StartLevel(int levelId)
        {
            var gridState = new GridStateEntity(levelId, _config.Width, _config.Height);
            var (refilledGrid, _) = _gridService.Refill(gridState, _cachedSeed, _config.AvailableTileTypes);

            _serverState = refilledGrid;
            return UniTask.FromResult((_config, _serverState));
        }

        public UniTask<GridStateEntity> SubmitMove(int x, int y)
        {
            var result = _evaluator.ResolveTap(_serverState, x, y, _config);

            if (result != null && result.Steps.Count > 0)
            {
                var lastStep = result.Steps[result.Steps.Count - 1];
                _serverState = lastStep.ResultingGridState;
            }

            return UniTask.FromResult(_serverState);
        }


        public void MockConfig(LevelConfigEntity config, int seed)
        {
            _config = config;
            _cachedSeed = seed;
        }
    }
}