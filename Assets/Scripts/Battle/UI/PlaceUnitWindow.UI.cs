using System;
using System.Linq;
using Battle.UI.Elements;
using TMPro;
using UnityEngine;

namespace Battle.UI
{
    public partial class PlaceUnitWindow
    {
        [Header("UI")] 
        [SerializeField] private TMP_InputField unitCountInputField;
        
        public void Cancel()
        {
            readyButton.interactable = true;
            background.SetActive(false);
            gameObject.SetActive(false);
        }

        public void Accept()
        {
            PlaceUnitOption selectedUnit = _unitTypeOptions.FirstOrDefault(placeUnitOption => placeUnitOption.Selected);
            if (selectedUnit == null)
            {
                Debug.Log("No unit was selected");
                return;
            }
            
            int selectedCount = Convert.ToInt32(unitCountInputField.text);
            if (selectedCount > selectedUnit.AvailableCount)
            {
                // TODO popup
                Debug.Log("Selected count exceeds available count");
                return;
            }
            if (selectedCount <= 0)
            {
                // TODO popup
                Debug.Log("Selected count is too low");
                return;
            }
            
            OnUnitPlaced?.Invoke(selectedUnit.UnitType, selectedCount);
            
            readyButton.interactable = true;
            background.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}