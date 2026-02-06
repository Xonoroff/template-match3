using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Managers
{
    public interface IMatch3Manager
    {
        UniTask<GridEntity> GetCurrentState();
        
        UniTask<GridEntity> HandleTap(int x, int y);
    }
}