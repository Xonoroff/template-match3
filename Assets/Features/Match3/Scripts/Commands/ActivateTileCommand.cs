using System;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;

namespace Features.Match3.Scripts.Commands
{
    [Serializable]
    public record ActivateTileCommand : ICommand
    {
        public GridStateEntity CurrentState { get; private set; }
        
        public LevelConfigEntity Config { get; private set; }
        
        public TileCoordinateEntity Coordinate { get; private set; }

        public ActivateTileCommand(TileCoordinateEntity coordinate, GridStateEntity currentState, LevelConfigEntity config)
        {
            Coordinate = coordinate; 
            CurrentState = currentState;
            Config = config;
        }
    }
}