using System;
using System.Collections.Generic;
using Features.Match3.Scripts.Domain; // For TileEntity info if needed, or mapped

namespace Features.Match3.Scripts.Views
{
    public struct BoardViewEntity
    {
        public int Width;
        public int Height;
        public TileEntity[] Tiles;
    }

    // A generic visual instruction
    public abstract class VisualAction { }

    public class SwapVisualAction : VisualAction
    {
        public int X1, Y1;
        public int X2, Y2;
        public bool IsReverse; // For failed swap
    }

    public class DestroyVisualAction : VisualAction
    {
        public int X, Y;
        public int UniqueId;
    }

    public class MoveVisualAction : VisualAction
    {
        public int FromX, FromY; // If -1, -1, it's a spawn (conceptually) but usually we separate spawn
        public int ToX, ToY;
        public int UniqueId;
        public TileEntity Tile; // To ensure view has data
    }

    public class SpawnVisualAction : VisualAction
    {
        public int X, Y;
        public TileEntity Tile;
    }
    
    // A group of actions that happen simultaneously
    public class VisualStep
    {
        public List<VisualAction> Actions = new List<VisualAction>();
    }

    // A sequence of steps that happen sequentially
    public class VisualSequence
    {
        public List<VisualStep> Steps = new List<VisualStep>();
    }
}
