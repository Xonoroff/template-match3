using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Entities.Configs;
using UnityEngine;

namespace Features.Match3.Scripts.Loaders
{
    public interface IMatch3ContentLoader : IDisposable
    {
        void Initialize(LevelConfigEntity config);
        UniTask<Sprite> LoadSpriteAsync(TileTypeIDEntity typeId);
    }
}