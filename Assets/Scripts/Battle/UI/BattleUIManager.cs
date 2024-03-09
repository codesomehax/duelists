using System.Collections.Generic;
using System.Linq;
using Factions;
using Units;
using UnityEngine;

namespace Battle.UI
{
    public class BattleUIManager : MonoBehaviour
    {
        [Header("Windows")] 
        [SerializeField] private PlaceUnitWindow placeUnitWindow;

        public void ShowPlaceUnitWindow(IDictionary<UnitType, PlaceUnitData> placeUnitData)
        {
            placeUnitWindow.Activate(placeUnitData);
        }
    }
}