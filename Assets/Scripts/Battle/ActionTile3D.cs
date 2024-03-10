using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battle
{
    public class ActionTile3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public static event Action<ActionTile3D> OnClick;

        [SerializeField] private Color placeholderColor;
        [SerializeField] private Color availableColor;
        [SerializeField] private Color attackColor;
        [SerializeField] private Color highlightColor;

        private readonly IDictionary<ActionTileState, Color> _colors = new Dictionary<ActionTileState, Color>(4);

        private ActionTileState _actionTileState;
        public ActionTileState ActionTileState
        {
            get => _actionTileState;
            set
            {
                SetColorByState(value);
                _actionTileState = value;
            }
        }
        public Vector3Int CellPosition { get; set; }

        private MeshRenderer _meshRenderer;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _colors[ActionTileState.Placeholder] = placeholderColor;
            _colors[ActionTileState.Available] = availableColor;
            _colors[ActionTileState.Attack] = attackColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Highlight();
        }

        private void Highlight()
        {
            _meshRenderer.material.color = highlightColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetColorByState(ActionTileState);
        }

        private void SetColorByState(ActionTileState state)
        {
            _meshRenderer.material.color = _colors[state];
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }
    }

    public enum ActionTileState
    {
        Placeholder,
        Available,
        Attack
    }
}