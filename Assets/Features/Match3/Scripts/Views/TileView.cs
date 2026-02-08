using System; // For Action
using Features.Match3.Scripts.Domain;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Match3.Scripts.Views
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TileView : MonoBehaviour, IPointerDownHandler
    {
        public int UniqueId { get; private set; }
        public event Action<TileView> OnClicked;

        [SerializeField] private SpriteRenderer _renderer;
        // In a real app, inject a helper or use Serialized Dictionary for config

        public void Initialize(TileViewEntity tileCommand)
        {
            UniqueId = tileCommand.UniqueId;
            UpdateVisuals(tileCommand); // Pass the whole entity to access sprite
        }

        public void UpdateVisuals(TileViewEntity tileCommand)
        {
            if (_renderer != null)
            {
                _renderer.sprite = tileCommand.Sprite;
            }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            OnClicked?.Invoke(this);
        }
    }
}
