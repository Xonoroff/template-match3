using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Services;
using UnityEngine; // For Debug.Log (or use custom logger)

namespace Features.Match3.Scripts.Managers
{
    public class Match3Manager : IMatch3Manager
    {
        //TODO: Add authorative state
        
        public GridEntity CurrentState { get; private set; }
        public LevelConfigEntity Config { get; private set; }
        
        // Dependencies
        private readonly ActivateTileHandler _activateTileHandler;

        public Match3Manager(
            ActivateTileHandler activateTileHandler,
            LevelConfigEntity config,
            GridEntity initialState)
        {
            _activateTileHandler = activateTileHandler;
            Config = config;
            CurrentState = initialState;
        }

        public UniTask<GridEntity> Initialize(int levelId)
        {
            //todo: Load level from client/backend
            return UniTask.FromResult(CurrentState);
        }
        
        public UniTask<GridEntity> GetCurrentState()
        {
            return UniTask.FromResult(CurrentState);
        }

        public async UniTask<GridEntity> HandleTap(int x, int y)
        {
            //TODO: Add validation
            
            var command = new ActivateTileCommand(x, y, CurrentState, Config);

            TrackCommand(command);
            
            var result = await _activateTileHandler.Handle(command);

            if (result.ResolvedSequence != null && result.ResolvedSequence.Steps.Count > 0)
            {
                // Update Authoritative State to the final state of the sequence
                var lastStep = result.ResolvedSequence.Steps[result.ResolvedSequence.Steps.Count - 1];
                CurrentState = lastStep.ResultingGrid;

            }

            return CurrentState;
        }

        private void TrackCommand(ICommand command)
        {
            //TODO: Send send analytics
            //TODO: Send to backend trackings and cheaters detection
        }
    }
}
