using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Entities;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;

namespace Features.Match3.Scripts.Managers
{
    public interface IMatch3Manager
    {
        UniTask<(LevelConfigEntity config, GridStateEntity state)> StartLevel(int levelId);

        UniTask<ResolveSequenceEntity> HandleTap(int x, int y);
    }
}