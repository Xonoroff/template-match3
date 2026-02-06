using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Managers;
using Features.Match3.Scripts.Views;

namespace Features.Match3.Scripts.Presenters
{
    public class BoardPresenter : IDisposable
    {
        private readonly IMatch3Manager _manager;
        private readonly BoardView _view;

        public BoardPresenter(IMatch3Manager manager, BoardView view)
        {
            _manager = manager;
            _view = view;

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

        private bool _isInputProcessing;

        private void HandleViewTileClicked(int x, int y)
        {
            if (_isInputProcessing) return;
            HandleViewTileClickedAsync(x,y).Forget();
        }

        private async UniTask HandleViewTileClickedAsync(int x, int y)
        {
            _isInputProcessing = true;
            try
            {
                var sequence = await _manager.HandleTap(x, y);
                await HandleSequenceResolved(sequence);
            }
            finally
            {
                _isInputProcessing = false;
            }
        }
        

        private async UniTask HandleSequenceResolved(ResolveSequence sequence)
        {
            if (sequence == null || sequence.Steps == null) return;

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
                            int width = step.ResultingGrid.Width;
                            int tileX = idx % width;
                            int tileY = idx / width;
                            
                            visualStep.Actions.Add(new DestroyVisualAction
                            {
                                X = tileX,
                                Y = tileY
                            });
                        }
                    }
                }
                else if (step is GravityStep gravity)
                {
                    foreach (var drop in gravity.Drops)
                    {
                        int width = step.ResultingGrid.Width;
                        int toX = drop.ToIndex % width;
                        int toY = drop.ToIndex / width;
                        
                        visualStep.Actions.Add(new MoveVisualAction
                        {
                            ToX = toX,
                            ToY = toY,
                            Tile = drop.Tile
                        });
                    }
                }
                else if (step is RefillStep refill)
                {
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

            await _view.ExecuteVisuals(visualSeq);
        }

        public void Dispose()
        {
            _view.OnTileClickedInternal -= HandleViewTileClicked;
        }
    }
}
