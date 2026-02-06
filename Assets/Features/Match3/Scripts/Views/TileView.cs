using Features.Match3.Scripts.Domain;
using UnityEngine;

namespace Features.Match3.Scripts.Views
{
    public class TileView : MonoBehaviour
    {
        public int UniqueId { get; private set; }
        
        [SerializeField] private SpriteRenderer _renderer;
        // In a real app, inject a helper or use Serialized Dictionary for config
        
        public void Initialize(TileEntity tileCommand)
        {
            UniqueId = tileCommand.UniqueId;
            UpdateVisuals(tileCommand.TypeId);
        }

        public void UpdateVisuals(TileTypeID typeId)
        {
            // Placeholder color logic until we have sprites
            if (_renderer != null)
            {
                _renderer.color = GetColorForType(typeId);
            }
        }

        private Color GetColorForType(TileTypeID type)
        {
             // Deterministic colors for debugging
             int hash = type.Value * 12345;
             return new Color(
                 ((hash >> 16) & 0xFF) / 255f,
                 ((hash >> 8) & 0xFF) / 255f,
                 (hash & 0xFF) / 255f
             );
        }

        public void SetSelected(bool isSelected)
        {
            if (_renderer != null)
            {
                // Simple feedback
                _renderer.transform.localScale = isSelected ? Vector3.one * 0.8f : Vector3.one;
            }
        }
    }
}
