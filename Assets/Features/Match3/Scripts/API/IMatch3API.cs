using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Services
{
    public interface IMatch3API
    {
        UniTask<(LevelConfigEntity config, GridEntity state)> StartLevel(int levelId);
        UniTask<GridEntity> SubmitMove(int x, int y);
    }
}
