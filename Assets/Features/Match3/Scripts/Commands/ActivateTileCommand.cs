using System;
using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Services
{
    [Serializable]
    public struct ActivateTileCommand : ICommand
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        
        public GridEntity CurrentState { get; private set; }
        
        public LevelConfigEntity Config { get; private set; }

        public ActivateTileCommand(int x, int y, GridEntity currentState, LevelConfigEntity config)
        {
            X = x;
            Y = y;
            CurrentState = currentState;
            Config = config;
        }
    }
}