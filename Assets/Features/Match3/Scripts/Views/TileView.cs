using System;
using Features.Match3.Scripts.Views.Entities; // For Action
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

        public void Initialize(TileViewEntity tileCommand)
        {
            UniqueId = tileCommand.UniqueId;
            UpdateVisuals(tileCommand);
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
