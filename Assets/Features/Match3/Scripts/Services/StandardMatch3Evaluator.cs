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

        public ResolveSequenceEntity ResolveTap(GridEntity startState, int x, int y, LevelConfigEntity config)
        {
            var sequence = new ResolveSequenceEntity();
            var currentGrid = startState.Clone();

            var matches = _matchService.GetConnectedTiles(currentGrid, x, y);

            if (matches.Count > 0)
            {
                int currentSeed = config.Seed + Environment.TickCount;

                sequence.Steps.Add(new MatchStepEntity { ResultingGrid = currentGrid.Clone(), Matches = matches });
                RemoveTiles(currentGrid, matches);

                ApplyGravityAndRefill(currentGrid, sequence, ref currentSeed, config);
            }

            return sequence;
        }

        private void RemoveTiles(GridEntity grid, List<MatchPatternEntity> matches)
        {
            foreach (var match in matches)
            {
                foreach (var coord in match.TileCoordinates)
                {
                    grid.SetTile(coord.X, coord.Y, TileEntity.Empty);
                }
            }
        }

        private void ApplyGravityAndRefill(GridEntity grid, ResolveSequenceEntity sequence, ref int seed, LevelConfigEntity config)
        {
            var (gridAfterGravity, gravityStep) = _gravityService.ApplyGravity(grid);

            sequence.Steps.Add(gravityStep);

            var (gridAfterRefill, refillStep) = _gridService.Refill(gridAfterGravity, seed++, config.AvailableColors);
            sequence.Steps.Add(refillStep);
        }
    }
}