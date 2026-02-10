using System;
using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Services
{
    [Serializable]
    public struct ActivateTileCommand : ICommand
    {
        
        public GridEntity CurrentState { get; private set; }
        
        public LevelConfigEntity Config { get; private set; }
        
        public TileCoordinateEntity Coordinate { get; private set; }

        public ActivateTileCommand(TileCoordinateEntity coordinate, GridEntity currentState, LevelConfigEntity config)
        {
            Coordinate = coordinate; 
            CurrentState = currentState;
            Config = config;
        }
    }
}