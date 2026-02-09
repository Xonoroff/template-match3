using System;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using UnityEngine;

namespace Features.Match3.Scripts.Services
{
    public interface IMatch3ContentLoader : IDisposable
    {
        void Initialize(LevelConfigEntity config);
        UniTask<Sprite> LoadSpriteAsync(TileTypeIDEntity typeId);
    }
}