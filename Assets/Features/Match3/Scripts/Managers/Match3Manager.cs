using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Services;
using UnityEngine; // For Debug.Log (or use custom logger)

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

        public async UniTask<GridEntity> StartLevel(int levelId)
        {
            var (config, state) = await _api.StartLevel(levelId);
            _cachedConfig = config;
            _cachedProvisionalState = state;
            return _cachedProvisionalState;
        }

        public async UniTask<ResolveSequence> HandleTap(int x, int y)
        {
            if (_cachedProvisionalState.Tiles == null)
            {
                return new ResolveSequence();
            }

            var command = new ActivateTileCommand(x, y, _cachedProvisionalState, _cachedConfig);
            
            var localResult = await _activateTileHandler.Handle(command);
            
            //TODO: Do not await for backend here and then - implement reactive subscription
            var authoritativeState = await _api.SubmitMove(x, y);
            
            _cachedProvisionalState = authoritativeState;
            
            if (localResult.ResolvedSequence != null && localResult.ResolvedSequence.Steps.Count > 0)
            {
                var lastStep = localResult.ResolvedSequence.Steps[localResult.ResolvedSequence.Steps.Count - 1];
                // We trust local simulation for visual flow, but eventually will sync with authoritative state
                // For now, let's keep authoritative state as the source of truth for the NEXT action, 
                // but return the local sequence for visualization
            }

            return localResult.ResolvedSequence;
        }
        
    }
}
