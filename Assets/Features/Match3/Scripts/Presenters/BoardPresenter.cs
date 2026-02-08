using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Managers;
using Features.Match3.Scripts.Services;
using Features.Match3.Scripts.Views;
using UnityEngine;

namespace Features.Match3.Scripts.Presenters
{
    public class BoardPresenter : IDisposable
    {
        private readonly IMatch3Manager _manager;
        private readonly BoardView _view;
        private readonly IMatch3ContentLoader _contentLoader;
        private Dictionary<TileTypeID, Sprite> _tileSpriteMap;

        public BoardPresenter(IMatch3Manager manager, BoardView view, IMatch3ContentLoader contentLoader)
        {
            _manager = manager;
            _view = view;
            _contentLoader = contentLoader;

            _view.OnTileClickedInternal += HandleViewTileClicked;
        }

        //TODO: Add cancellation tokens
        public async UniTask StartLevelAsync(int levelId)
        {
            var (config, state) = await _manager.StartLevel(levelId);
            _contentLoader.Initialize(config);

            _tileSpriteMap = new Dictionary<TileTypeID, Sprite>();
            foreach (var type in config.AvailableTileTypes)
            {
                if (!_tileSpriteMap.ContainsKey(type))
                {
                    var sprite = await _contentLoader.LoadSpriteAsync(type);
                    _tileSpriteMap[type] = sprite;
                }
            }

            // Map TileEntity to TileViewEntity
            var viewTiles = new TileViewEntity[state.Tiles.Length];
            for (int i = 0; i < state.Tiles.Length; i++)
            {
                // Fallback to null if not found (or should we log error? Loader logs warnings)
                _tileSpriteMap.TryGetValue(state.Tiles[i].TypeId, out var sprite);

                viewTiles[i] = new TileViewEntity
                {
                    UniqueId = state.Tiles[i].UniqueId,
                    TypeId = state.Tiles[i].TypeId,
                    Sprite = sprite
                };
            }

            var viewEntity = new BoardViewEntity
            {
                Width = state.Width,
                Height = state.Height,
                Tiles = viewTiles
            };
            _view.Initialize(viewEntity);
        }

        private bool _isInputProcessing;

        private void HandleViewTileClicked(int x, int y)
        {
            if (_isInputProcessing) return;
            HandleViewTileClickedAsync(x, y).Forget();
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

                        _tileSpriteMap.TryGetValue(drop.Tile.TypeId, out var sprite);

                        visualStep.Actions.Add(new MoveVisualAction
                        {
                            ToX = toX,
                            ToY = toY,
                            Tile = new TileViewEntity
                            {
                                UniqueId = drop.Tile.UniqueId,
                                TypeId = drop.Tile.TypeId,
                                Sprite = sprite
                            }
                        });
                    }
                }
                else if (step is RefillStep refill)
                {
                    var grid = step.ResultingGrid;
                    foreach (var newTile in refill.NewTiles)
                    {
                        // Find newTile in grid
                        for (int i = 0; i < grid.Tiles.Length; i++)
                        {
                            if (grid.Tiles[i].UniqueId == newTile.UniqueId)
                            {
                                _tileSpriteMap.TryGetValue(newTile.TypeId, out var sprite);

                                visualStep.Actions.Add(new SpawnVisualAction
                                {
                                    X = i % grid.Width,
                                    Y = i / grid.Width,
                                    Tile = new TileViewEntity
                                    {
                                        UniqueId = newTile.UniqueId,
                                        TypeId = newTile.TypeId,
                                        Sprite = sprite
                                    }
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
