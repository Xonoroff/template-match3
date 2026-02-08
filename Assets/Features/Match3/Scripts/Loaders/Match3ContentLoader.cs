using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

namespace Features.Match3.Scripts.Services
{
    public class Match3ContentLoader : MonoBehaviour, IMatch3ContentLoader
    {
        private SpriteAtlas _cachedAtlas;
        private Dictionary<int, Sprite> _spriteCache = new Dictionary<int, Sprite>();
        private AsyncOperationHandle<SpriteAtlas> _atlasHandle;
        private LevelConfigEntity _config;

        public void Initialize(LevelConfigEntity config)
        {
            _config = config;
        }

        public async UniTask<Sprite> LoadSpriteAsync(TileTypeID typeId)
        {
            if (_spriteCache.TryGetValue(typeId.Value, out var cachedSprite))
            {
                return cachedSprite;
            }

            if (_cachedAtlas == null)
            {
                if (string.IsNullOrEmpty(_config.AtlasAddress))
                {
                    Debug.LogError("Match3ContentLoader: AtlasAddress is null or empty in LevelConfig!");
                    return null;
                }

                _atlasHandle = Addressables.LoadAssetAsync<SpriteAtlas>(_config.AtlasAddress);
                _cachedAtlas = await _atlasHandle.Task.AsUniTask();

                if (_cachedAtlas == null)
                {
                    Debug.LogError($"Match3ContentLoader: Failed to load atlas at address '{_config.AtlasAddress}'");
                    return null;
                }
            }

            // Fallback if prefix is missing? Or just use "Tile_" default?
            // User said "move it to level config", so we expect it there.
            var prefix = string.IsNullOrEmpty(_config.SpritePrefix) ? "Tile_" : _config.SpritePrefix;
            var spriteName = $"{prefix}{typeId.Value}";
            var sprite = _cachedAtlas.GetSprite(spriteName);

            if (sprite != null)
            {
                _spriteCache[typeId.Value] = sprite;
            }
            else
            {
                Debug.LogWarning($"Match3ContentLoader: Sprite not found for type: {typeId.Value} (Name: {spriteName})");
            }

            return sprite;
        }

        public void Unload()
        {
            _spriteCache.Clear();
            if (_cachedAtlas != null)
            {
                Addressables.Release(_atlasHandle);
                _cachedAtlas = null;
            }
        }

        private void OnDestroy()
        {
            Unload();
        }
    }
}
