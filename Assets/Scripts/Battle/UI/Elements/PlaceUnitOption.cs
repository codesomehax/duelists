using System;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI.Elements
{
    public partial class PlaceUnitOption : MonoBehaviour
    {
        private static event Action OnSelected; 

        [SerializeField] private UnitType unitType;

        public UnitType UnitType => unitType;

        public string UnitName
        {
            set => SetUnitNameText(value);
        }

        private int _availableCount;
        public int AvailableCount
        {
            get => _availableCount;
            set
            {
                _availableCount = value;
                SetUnitCountText(value);
            }
        }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                SetImageColor(value);
            }
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            OnSelected += Unselect;
        }

        private void Unselect()
        {
            Selected = false;
        }

        private void OnDisable()
        {
            Unselect();
        }
    }
}