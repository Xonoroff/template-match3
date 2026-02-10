using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.API;
using Features.Match3.Scripts.Commands;
using Features.Match3.Scripts.Entities;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;
using UnityEngine;

namespace Features.Match3.Scripts.Managers
{
    public class Match3Manager : IMatch3Manager
    {
        private GridStateEntity _cachedProvisionalState;
        private LevelConfigEntity _cachedConfig;

        private readonly ActivateTileHandler _activateTileHandler;
        private readonly IMatch3API _api;

        public Match3Manager(
            ActivateTileHandler activateTileHandler,
            IMatch3API api)
        {
            _activateTileHandler = activateTileHandler;
            _api = api;
        }

        public async UniTask<(LevelConfigEntity config, GridStateEntity state)> StartLevel(int levelId)
        {
            var (config, state) = await _api.StartLevel(levelId);
            _cachedConfig = config;
            _cachedProvisionalState = state;
            return (_cachedConfig, _cachedProvisionalState);
        }

        public event Action<GridStateEntity> OnGameStateUpdated;

        public async UniTask<ResolveSequenceEntity> HandleTap(int x, int y)
        {
            if (_cachedProvisionalState.Tiles == null)
            {
                return new ResolveSequenceEntity();
            }

            var coordinates = new TileCoordinateEntity(x, y);
            var command = new ActivateTileCommand(coordinates, _cachedProvisionalState, _cachedConfig);

            var localResult = await _activateTileHandler.Handle(command);

            //SyncGameStateAsync(x, y).Forget();

            return localResult.ResolvedSequence;
        }

        private async UniTaskVoid SyncGameStateAsync(int x, int y)
        {
            try
            {
                var authoritativeState = await _api.SubmitMove(x, y);
                _cachedProvisionalState = authoritativeState;
                OnGameStateUpdated?.Invoke(_cachedProvisionalState);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to sync game state: {e.Message}");
            }
        }

    }
}
