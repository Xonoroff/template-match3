using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Services;
using UnityEngine;

namespace Features.Match3.Scripts.Managers
{
    public class Match3Manager : IMatch3Manager
    {
        private GridEntity _cachedProvisionalState;
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

        public async UniTask<(LevelConfigEntity config, GridEntity state)> StartLevel(int levelId)
        {
            var (config, state) = await _api.StartLevel(levelId);
            _cachedConfig = config;
            _cachedProvisionalState = state;
            return (_cachedConfig, _cachedProvisionalState);
        }

        public event Action<GridEntity> OnGameStateUpdated;

        public async UniTask<ResolveSequenceEntity> HandleTap(int x, int y)
        {
            if (_cachedProvisionalState.Tiles == null)
            {
                return new ResolveSequenceEntity();
            }

            var command = new ActivateTileCommand(x, y, _cachedProvisionalState, _cachedConfig);

            var localResult = await _activateTileHandler.Handle(command);

            SyncGameStateAsync(x, y).Forget();

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
