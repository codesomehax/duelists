using System;
using System.Collections.Generic;
using System.Linq;
using Factions;
using Units;
using UnityEngine;

namespace Battle.UI
{
    public class BattleUIManager : MonoBehaviour
    {
        public event Action<UnitType, int> OnUnitPlaced; 

        [Header("Windows")] 
        [SerializeField] private PlaceUnitWindow placeUnitWindow;

        private void Awake()
        {
            placeUnitWindow.OnUnitPlaced += TriggerOnUnitPlaced;
        }

        private void TriggerOnUnitPlaced(UnitType unitType, int unitCount)
        {
            OnUnitPlaced?.Invoke(unitType, unitCount);
        }

        public void ShowPlaceUnitWindow(IDictionary<UnitType, PlaceUnitData> placeUnitData)
        {
            placeUnitWindow.Activate(placeUnitData);
        }
    }
}