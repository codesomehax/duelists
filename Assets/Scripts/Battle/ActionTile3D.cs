using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battle
{
    public class ActionTile3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public static event Action<ActionTile3D> OnMouseEnter;
        public static event Action<ActionTile3D> OnMouseExit;
        public static event Action<ActionTile3D> OnClick;

        public Color Color
        {
            get => _meshRenderer.material.color;
            set
            {
                _previousColor = Color;
                _meshRenderer.material.color = value;
            }
        }

        public bool Occupied { get; set; }

        private MeshRenderer _meshRenderer;

        private Color _previousColor;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _previousColor = Color;
        }

        public void SetPreviousColor()
        {
            Color = _previousColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMouseEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnMouseExit?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }
    }
}