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
            var sequence = new ResolveSequence { InitialState = startState };
            var currentGrid = startState.Clone();

            // 1. Check Matches (Connected Tiles)
            var matches = _matchService.GetConnectedTiles(currentGrid, x, y);
            
             // 2. Resolve if matches found
            if (matches.Count > 0)
            {
                int safetyCounter = 0;
                int currentSeed = config.Seed + Environment.TickCount; 

                // Process initial match
                 sequence.Steps.Add(new MatchStep { ResultingGrid = currentGrid.Clone(), Matches = matches });
                 RemoveTiles(currentGrid, matches);

                 // Apply Gravity & Refill immediately after first match
                 ApplyGravityAndRefill(currentGrid, sequence, ref currentSeed, config);
                
                // Optional: Check for chain reactions? 
                // The plan said: "Automatic matches after refill are NOT enabled by default unless requested".
                // So we stop here unless we want chains. 
                // However, standard Match3 usually has chains. "Blast" mechanics often don't. 
                // User said: "then calculations calculate adjusted tiles and destroys them. And then adds new cells for empty grid cells. And then view just animates them."
                // "Adjusted tiles" -> Gravity.
                // "Adds new cells" -> Refill.
                // The prompt didn't explicitly forbid chains, but "Blast" usually is single-step.
                // Let's stick to single step per plan.
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
            // Gravity
            var (gridAfterGravity, gravityStep) = _gravityService.ApplyGravity(grid);
            
            // Note: In C# structs are value types, so 'grid' local var needs update if we want to use it again, 
            // but here we just pass 'gridAfterGravity' to Refill.
            // However, we need to update 'grid' if we were looping. 
            // Be careful with struct copying. 'ApplyGravity' returns a NEW grid.
            // We should ideally update the reference if we were in a loop.
            // Since we are not looping for chains, it's fine.
            
            sequence.Steps.Add(gravityStep);

            // Refill
            var (gridAfterRefill, refillStep) = _gridService.Refill(gridAfterGravity, seed++, config.AvailableTileTypes);
            sequence.Steps.Add(refillStep);
        }
    }
}