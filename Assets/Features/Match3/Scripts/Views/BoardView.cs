using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.Match3.Scripts.Domain;
using UnityEngine;

namespace Features.Match3.Scripts.Views
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private TileView _tilePrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private float _cellSize = 1.0f;
        
        // State
        private Dictionary<int, TileView> _activeTiles = new Dictionary<int, TileView>();
        private BoardViewEntity _currentContext;
        private int _selectedX=-1, _selectedY=-1;

        // Events
        public event Action<int, int> OnTileClickedInternal;

        public void Initialize(BoardViewEntity context)
        {
            _currentContext = context;
            
            // Clear existing
            foreach(Transform child in _container) Destroy(child.gameObject);
            _activeTiles.Clear();

            // Spawn grid
            for (int i = 0; i < context.Tiles.Length; i++)
            {
                var tileData = context.Tiles[i];
                if (tileData.IsEmpty) continue;

                int x = i % context.Width;
                int y = i / context.Width;

                SpawnTile(tileData, x, y);
            }
        }

        private TileView SpawnTile(TileEntity tileData, int x, int y)
        {
            var tile = Instantiate(_tilePrefab, _container);
            tile.transform.localPosition = GridToLocal(x, y);
            tile.Initialize(tileData);
            tile.OnClicked += OnTileClickedHandler;
            _activeTiles[tileData.UniqueId] = tile;
            return tile;
        }

        private void OnTileClickedHandler(TileView tile)
        {
            var gridPos = LocalToGrid(tile.transform.localPosition);
            
            // Validate bounds
            if (gridPos.x >= 0 && gridPos.x < _currentContext.Width && 
                gridPos.y >= 0 && gridPos.y < _currentContext.Height)
            {
                OnTileClickedInternal?.Invoke(gridPos.x, gridPos.y);
            }
        }

        private Vector3 GridToLocal(int x, int y)
        {
            return new Vector3(x * _cellSize, y * _cellSize, 0);
        }

        public void UpdateSelection(int x, int y)
        {
            // Reset old selection
            // We need to find the tile at selectedX, selectedY. 
            // _activeTiles is by ID. We don't have efficient (x,y) -> ID map here without search or maintenance.
            // For now, let's iterate or maintain a secondary map.
            // Or since selection is purely visual, maybe just highlight uniqueID if we knew it?
            // "UpdateSelection(x, y)" comes from Presenter.
            
            // Simple visual approach:
            _selectedX = x;
            _selectedY = y;
            // Iterate all active tiles to reset scale? Slow.
            // But we don't have the map. 
            // Let's rely on checking position distance or keep a map.
            // For MVP:
            foreach(var kvp in _activeTiles)
            {
                var t = kvp.Value;
                var gridPos = LocalToGrid(t.transform.localPosition);
                bool isTarget = (gridPos.x == x && gridPos.y == y);
                t.SetSelected(isTarget);
            }
        }

        private Vector2Int LocalToGrid(Vector3 localPos)
        {
            return new Vector2Int(Mathf.RoundToInt(localPos.x / _cellSize), Mathf.RoundToInt(localPos.y / _cellSize));
        }

        public async UniTask ExecuteVisuals(VisualSequence sequence)
        {
            foreach (var step in sequence.Steps)
            {
                var tasks = new List<UniTask>();

                foreach (var action in step.Actions)
                {
                    if (action is DestroyVisualAction destroy)
                    {
                        var t = GetTileAt(destroy.X, destroy.Y); // Or by UniqueID if provided
                        if (t)
                        {
                            _activeTiles.Remove(t.UniqueId);
                            // Unsubscribe before destroying to be safe, though Destroy clears generic invokes, C# events persist if not cleared but the object is dead. 
                            // Strong ref is Tile -> Board (Deletage). Board -> Tile (Ref). Tile is dead. GC handles it.
                            t.OnClicked -= OnTileClickedHandler; 
                            tasks.Add(AnimateDestroy(t));
                        }
                    }
                    else if (action is MoveVisualAction move)
                    {
                        if (_activeTiles.TryGetValue(move.Tile.UniqueId, out var tile))
                        {
                             // Move
                             tasks.Add(AnimateMove(tile, move.ToX, move.ToY));
                        }
                    }
                    else if (action is SpawnVisualAction spawn)
                    {
                        // Spawn above
                        var t = SpawnTile(spawn.Tile, spawn.X, spawn.Y + 2); // Start higher
                        tasks.Add(AnimateMove(t, spawn.X, spawn.Y));
                        // Or simple spawn
                    }
                }

                if (tasks.Count > 0)
                {
                    await UniTask.WhenAll(tasks);
                }
            }
        }

        private TileView GetTileAt(int x, int y)
        {
            // Ideally optimize this lookup
            foreach (var kvp in _activeTiles)
            {
                 var gridPos = LocalToGrid(kvp.Value.transform.localPosition);
                 if (gridPos.x == x && gridPos.y == y) return kvp.Value;
            }
            return null;
        }

        private async UniTask AnimateSwap(TileView t1, TileView t2)
        {
            var p1 = t1.transform.localPosition;
            var p2 = t2.transform.localPosition;
            float duration = 0.2f;
            float elapsed = 0;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                t1.transform.localPosition = Vector3.Lerp(p1, p2, t);
                t2.transform.localPosition = Vector3.Lerp(p2, p1, t);
                await UniTask.Yield();
            }
            t1.transform.localPosition = p2;
            t2.transform.localPosition = p1;
        }

        private async UniTask AnimateMove(TileView t, int x, int y)
        {
            var start = t.transform.localPosition;
            var end = GridToLocal(x, y);
             float duration = 0.2f;
            float elapsed = 0;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float tVal = elapsed / duration;
                t.transform.localPosition = Vector3.Lerp(start, end, tVal);
                await UniTask.Yield();
            }
            t.transform.localPosition = end;
        }

        private async UniTask AnimateDestroy(TileView t)
        {
            // Scale down
            float duration = 0.2f;
             float elapsed = 0;
            var startScale = t.transform.localScale;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float tVal = elapsed / duration;
                t.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, tVal);
                await UniTask.Yield();
            }
            
            Destroy(t.gameObject);
        }
    }
}
