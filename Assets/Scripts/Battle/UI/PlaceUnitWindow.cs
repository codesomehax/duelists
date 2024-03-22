using System;
using System.Collections.Generic;
using Battle.UI.Elements;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    public partial class PlaceUnitWindow : MonoBehaviour
    {
        public event Action<UnitType, int> OnUnitPlaced;

        [SerializeField] private GameObject background;
        [SerializeField] private Button readyButton;
        
        private PlaceUnitOption[] _unitTypeOptions;
        
        public void Activate(IDictionary<UnitType, PlaceUnitData> placeUnitDataDictionary)
        {
            gameObject.SetActive(true);
            foreach (PlaceUnitOption unitTypeOption in _unitTypeOptions)
            {
                PlaceUnitData placeUnitData = placeUnitDataDictionary[unitTypeOption.UnitType];
                unitTypeOption.UnitName = placeUnitData.Name;
                unitTypeOption.AvailableCount = placeUnitData.Count;
            }
        }

        private void Awake()
        {
            _unitTypeOptions = GetComponentsInChildren<PlaceUnitOption>();
        }
    }

    public struct PlaceUnitData
    {
        public string Name;
        public int Count;
    }
}