using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Managers;
using Features.Match3.Scripts.Views;

namespace Features.Match3.Scripts.Presenters
{
    public class BoardPresenter
    {
        private readonly IMatch3Manager _manager;
        private readonly BoardView _view;

        public BoardPresenter(IMatch3Manager manager, BoardView view)
        {
            _manager = manager;
            _view = view;

            // View Events
            _view.OnTileClickedInternal += HandleViewTileClicked;
        }

        //TODO: Add cancellation tokens
        public async UniTask StartLevelAsync(int levelId)
        {
            var state = await _manager.StartLevel(levelId);
            var viewEntity = new BoardViewEntity
            {
                Width = state.Width,
                Height = state.Height,
                Tiles = state.Tiles
            };
            _view.Initialize(viewEntity);
        }

        private void HandleViewTileClicked(int x, int y)
        {
            HandleViewTileClickedAsync(x,y).Forget();
        }

        private async UniTask HandleViewTileClickedAsync(int x, int y)
        {
            //TODO: ResolveSequence
            var result = await _manager.HandleTap(x, y);
        }
        

        private void HandleSequenceResolved(ResolveSequence sequence)
        {
            var visualSeq = new VisualSequence();

            foreach (var step in sequence.Steps)
            {
                var visualStep = new VisualStep();

                if (step is MatchStep match)
                {
                    foreach (var pattern in match.Matches)
                    {
                        foreach (var idx in pattern.TileIndices)
                        {
                            // Convert idx to x,y
                            // This requires knowing grid width. We can get it from Manager or Step.ResultingGrid.
                            int width = step.ResultingGrid.Width; // Assuming ResultingGrid is set
                            int x = idx % width;
                            int y = idx / width;
                            
                            visualStep.Actions.Add(new DestroyVisualAction
                            {
                                X = x,
                                Y = y,
                                // UniqueId? Need to get it from previous state or we rely on View's map
                                // View knows what visual is at (X, Y)
                            });
                        }
                    }
                }
                else if (step is GravityStep gravity)
                {
                    foreach (var drop in gravity.Drops)
                    {
                        int width = step.ResultingGrid.Width;
                        int fromX = drop.FromIndex % width;
                        int fromY = drop.FromIndex / width;
                        int toX = drop.ToIndex % width;
                        int toY = drop.ToIndex / width;
                        
                        visualStep.Actions.Add(new MoveVisualAction
                        {
                            FromX = fromX, 
                            FromY = fromY,
                            ToX = toX,
                            ToY = toY,
                            Tile = drop.Tile // Contains UniqueId to track
                        });
                    }
                }
                else if (step is RefillStep refill)
                {
                    // RefillStep in Domain stores "NewTiles". We need to know where they spawn.
                    // The StandardGridSystem puts them in.
                    // We can diff the grid or infer from DropData/Empty slots.
                    // StandardGridSystem implementation:
                    // "newTiles.Add(newTile)"
                    // But where are they? 
                    // Let's improve RefillStep to include index or check the grid.
                    // For now, let's look at ResultingGrid and assume any tile at (x,y) that wasn't there before is a Spawn?
                    // Or iterate ResultingGrid: any tile that has UniqueID in NewTiles list logic.
                    
                    // Refill Logic in Presenter:
                    // Finds where the new tiles ended up.
                    // Since gravity already happened, they fill top slots?
                    // Actually StandardGridSystem just fills empty slots.
                    // Gravity usually fills bottom slots, leaving top slots empty.
                    // Then Refill fills those top slots.
                    
                    var grid = step.ResultingGrid;
                    foreach (var newTile in refill.NewTiles)
                    {
                         // Find newTile in grid
                         for(int i=0; i<grid.Tiles.Length; i++)
                         {
                             if (grid.Tiles[i].UniqueId == newTile.UniqueId)
                             {
                                 visualStep.Actions.Add(new SpawnVisualAction
                                 {
                                     X = i % grid.Width,
                                     Y = i / grid.Width,
                                     Tile = newTile
                                 });
                                 break;
                             }
                         }
                    }
                }

                visualSeq.Steps.Add(visualStep);
            }

            _view.ExecuteVisuals(visualSeq);
        }
    }
}
