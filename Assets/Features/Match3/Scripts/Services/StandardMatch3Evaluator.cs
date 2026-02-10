using System;
using System.Collections.Generic;
using Features.Match3.Scripts.Entities;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;
using Features.Match3.Scripts.Entities.Steps;

namespace Features.Match3.Scripts.Services
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

        public ResolveSequenceEntity ResolveTap(GridStateEntity gridState, int x, int y, LevelConfigEntity config)
        {
            var sequence = new ResolveSequenceEntity();

            var matches = _matchService.GetConnectedTiles(gridState, x, y);

            if (matches.Count > 0)
            {
                int currentSeed = config.Seed + Environment.TickCount;

                sequence.Steps.Add(new MatchStepEntity(gridState, matches));
                RemoveTiles(gridState, matches);

                ApplyGravityAndRefill(gridState, sequence, ref currentSeed, config);
            }

            return sequence;
        }

        private void RemoveTiles(GridStateEntity gridState, List<MatchPatternEntity> matches)
        {
            foreach (var match in matches)
            {
                foreach (var coord in match.TileCoordinates)
                {
                    gridState.SetTile(coord.X, coord.Y, TileEntity.Empty(coord));
                }
            }
        }

        private void ApplyGravityAndRefill(GridStateEntity gridState, ResolveSequenceEntity sequence, ref int seed, LevelConfigEntity config)
        {
            var (gridAfterGravity, gravityStep) = _gravityService.ApplyGravity(gridState);

            sequence.Steps.Add(gravityStep);

            var (gridAfterRefill, refillStep) = _gridService.Refill(gridAfterGravity, seed++, config.AvailableTileTypes);
            sequence.Steps.Add(refillStep);
        }
    }
}