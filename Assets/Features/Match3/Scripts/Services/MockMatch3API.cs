using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Services
{
    public class MockMatch3API : IMatch3API
    {
        private GridEntity _serverState;
        private LevelConfigEntity _config;

        private int _cachedSeed;
        
        // Reuse the evaluator logic to simulate "server-side" calculation
        private readonly IMatch3Evaluator _evaluator;
        private readonly IGridService _gridService;

        public MockMatch3API(IMatch3Evaluator evaluator, IGridService gridService)
        {
            _evaluator = evaluator;
            _gridService = gridService;
        }
        

        public void MockConfig(LevelConfigEntity config, int seed)
        {
            _config = config;
            _cachedSeed = seed;
        }

        //TODO: Is can also send back invalid code. E.g. user is cheater.
        public UniTask<(LevelConfigEntity config, GridEntity state)> StartLevel(int levelId)
        {
            var gridState = new GridEntity(_config.Width, _config.Height);
            var (refilledGrid, _) = _gridService.Refill(gridState, _cachedSeed, _config.AvailableTileTypes);

            _serverState = refilledGrid;
            return UniTask.FromResult((_config, _serverState));
        }

        public async UniTask<GridEntity> SubmitMove(int x, int y)
        {
            var result = _evaluator.ResolveTap(_serverState, x, y, _config);
            
            if (result != null && result.Steps.Count > 0)
            {
                 var lastStep = result.Steps[result.Steps.Count - 1];
                 _serverState = lastStep.ResultingGrid;
            }

            return _serverState;
        }
    }
}
