using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Entities.Configs;
using Features.Match3.Scripts.Entities.States;

namespace Features.Match3.Scripts.API
{
    public interface IMatch3API
    {
        UniTask<(LevelConfigEntity config, GridStateEntity state)> StartLevel(int levelId);
        UniTask<GridStateEntity> SubmitMove(int x, int y);
    }
}