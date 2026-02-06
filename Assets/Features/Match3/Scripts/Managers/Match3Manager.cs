using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Services;
using UnityEngine; // For Debug.Log (or use custom logger)

namespace Features.Match3.Scripts.Managers
{
    public class Match3Manager : IMatch3Manager
    {
        public GridEntity CurrentState { get; private set; }
        public LevelConfigEntity Config { get; private set; }
        
        // Dependencies
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
            Config = config;
            CurrentState = state;
            return CurrentState;
        }

        public async UniTask<ResolveSequence> HandleTap(int x, int y)
        {
            if (CurrentState.Tiles == null)
            {
                return new ResolveSequence();
            }

            var command = new ActivateTileCommand(x, y, CurrentState, Config);
            
            var localResult = await _activateTileHandler.Handle(command);
            
            //TODO: Do not await for backend here and then - implement reactive subscription
            var authoritativeState = await _api.SubmitMove(x, y);
            
            CurrentState = authoritativeState;
            
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
