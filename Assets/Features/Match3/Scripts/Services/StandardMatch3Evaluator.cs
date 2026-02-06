using System;
using System.Collections.Generic;

namespace Features.Match3.Scripts.Domain
{
    public class StandardMatch3Evaluator : IMatch3Evaluator
    {
        private readonly IGridService _gridService;
        private readonly IMatchService _matchService;
        private readonly IGravityService _gravityService;

        public StandardMatch3Evaluator(IGridService grid, IMatchService match, IGravityService gravity)
        {
            _gridService = grid;
            _matchService = match;
            _gravityService = gravity;
        }

        public ResolveSequence ResolveTap(GridEntity startState, int x, int y, LevelConfigEntity config)
        {
            var sequence = new ResolveSequence();
            var currentGrid = startState.Clone();

            var matches = _matchService.GetConnectedTiles(currentGrid, x, y);
            
            if (matches.Count > 0)
            {
                int currentSeed = config.Seed + Environment.TickCount; 

                 sequence.Steps.Add(new MatchStep { ResultingGrid = currentGrid.Clone(), Matches = matches });
                 RemoveTiles(currentGrid, matches);

                 ApplyGravityAndRefill(currentGrid, sequence, ref currentSeed, config);
            }
            
            return sequence;
        }

        private void RemoveTiles(GridEntity grid, List<MatchPattern> matches)
        {
            foreach (var match in matches)
            {
                foreach (var idx in match.TileIndices)
                {
                    int x = idx % grid.Width;
                    int y = idx / grid.Width;
                    grid.SetTile(x, y, TileEntity.Empty);
                }
            }
        }

        private void ApplyGravityAndRefill(GridEntity grid, ResolveSequence sequence, ref int seed, LevelConfigEntity config)
        {
            var (gridAfterGravity, gravityStep) = _gravityService.ApplyGravity(grid);
            
            sequence.Steps.Add(gravityStep);

            var (gridAfterRefill, refillStep) = _gridService.Refill(gridAfterGravity, seed++, config.AvailableTileTypes);
            sequence.Steps.Add(refillStep);
        }
    }
}