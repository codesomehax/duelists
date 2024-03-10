using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Battle.UI
{
    public class BattleUIManager : MonoBehaviour
    {
        public event Action<UnitType, int> OnUnitPlaced;
        public event Action OnUnitRemovalConfirmed;

        [Header("Windows")] 
        [SerializeField] private PlaceUnitWindow placeUnitWindow;
        [SerializeField] private UnitRemovalConfirmationWindow unitRemovalConfirmationWindow;

        private void Awake()
        {
            placeUnitWindow.OnUnitPlaced += TriggerOnUnitPlaced;
            unitRemovalConfirmationWindow.OnRemovalConfirmed += TriggerOnUnitRemovalConfirmed;
        }

        private void TriggerOnUnitPlaced(UnitType unitType, int unitCount)
        {
            OnUnitPlaced?.Invoke(unitType, unitCount);
        }

        private void TriggerOnUnitRemovalConfirmed()
        {
            OnUnitRemovalConfirmed?.Invoke();
        }

        public void ShowPlaceUnitWindow(IDictionary<UnitType, PlaceUnitData> placeUnitData)
        {
            placeUnitWindow.Activate(placeUnitData);
        }

        public void AskRemoveUnit()
        {
            unitRemovalConfirmationWindow.gameObject.SetActive(true);
        }
    }
}