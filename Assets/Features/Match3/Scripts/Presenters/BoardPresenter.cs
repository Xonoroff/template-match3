using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Managers;
using Features.Match3.Scripts.Presenters.StepConverters;
using Features.Match3.Scripts.Services;
using Features.Match3.Scripts.Views;
using UnityEngine;

namespace Features.Match3.Scripts.Presenters
{

    public class BoardPresenter : IDisposable
    {
        private ILogger _logger;
        private readonly IMatch3Manager _manager;
        private readonly BoardView _view;
        private readonly IMatch3ContentLoader _contentLoader;
        private readonly StepVisualConverterRegistry _converterRegistry;
        private Dictionary<TileTypeIDEntity, Sprite> _tileSpriteMap;

        private bool _isInputProcessing;

        public BoardPresenter(IMatch3Manager manager, BoardView view, IMatch3ContentLoader contentLoader, ILogger logger)
        {
            _manager = manager;
            _view = view;
            _contentLoader = contentLoader;
            _logger = logger;

            _converterRegistry = new StepVisualConverterRegistry();

            _view.OnTileClickedInternal += HandleViewTileClicked;
        }

        public async UniTask StartLevelAsync(int levelId, CancellationToken ct = default)
        {
            var (config, state) = await _manager.StartLevel(levelId);
            if (ct.IsCancellationRequested)
            {
                _logger.LogWarning("[Match3Module]", $"Cancellation requested while starting level {levelId}");
                return;
            }

            _contentLoader.Initialize(config);

            _tileSpriteMap = new Dictionary<TileTypeIDEntity, Sprite>();

            var uniqueTypes = new HashSet<TileTypeIDEntity>(config.AvailableColors);
            var typesList = new List<TileTypeIDEntity>(uniqueTypes);
            var loadTasks = new List<UniTask<Sprite>>(typesList.Count);

            foreach (var type in typesList)
            {
                loadTasks.Add(_contentLoader.LoadSpriteAsync(type));
            }

            var loadedSprites = await UniTask.WhenAll(loadTasks);

            for (int i = 0; i < loadedSprites.Length; i++)
            {
                _tileSpriteMap[typesList[i]] = loadedSprites[i];
            }

            var viewTiles = new TileViewEntity[state.Tiles.Length];
            for (int i = 0; i < state.Tiles.Length; i++)
            {
                _tileSpriteMap.TryGetValue(state.Tiles[i].Type, out var sprite);

                viewTiles[i] = new TileViewEntity
                {
                    UniqueId = state.Tiles[i].UniqueId,
                    TypeId = state.Tiles[i].Type,
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


        private async UniTask HandleSequenceResolved(ResolveSequenceEntity sequence)
        {
            if (sequence == null || sequence.Steps == null) return;

            var visualSeq = new VisualSequence();

            foreach (var step in sequence.Steps)
            {
                var converter = _converterRegistry.GetConverter(step);
                var visualStep = converter.Convert(step, _tileSpriteMap);
                visualSeq.Steps.Add(visualStep);
            }

            await _view.ExecuteVisuals(visualSeq);
        }

        public void Dispose()
        {
            _contentLoader.Dispose();
            _view.OnTileClickedInternal -= HandleViewTileClicked;
        }
    }
}
