using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using UnityEngine;

namespace Features.Match3.Scripts.Services
{
    public interface IMatch3ContentLoader
    {
        void Initialize(LevelConfigEntity config);
        UniTask<Sprite> LoadSpriteAsync(TileTypeID typeId);
        void Unload();
    }
}