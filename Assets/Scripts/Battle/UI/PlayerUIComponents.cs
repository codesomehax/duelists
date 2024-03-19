using System;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    public class PlayerUIComponents : MonoBehaviour
    {
        public event Action OnCastSpellButtonClick;
        
        [SerializeField] private PlaceUnitWindow placeUnitWindow;
        [SerializeField] private UnitRemovalConfirmationWindow unitRemovalConfirmationWindow;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button castSpellButton;

        public PlaceUnitWindow PlaceUnitWindow => placeUnitWindow;
        public UnitRemovalConfirmationWindow UnitRemovalConfirmationWindow => unitRemovalConfirmationWindow;
        public Button ReadyButton => readyButton;
        public Button EndTurnButton => endTurnButton;
        public Button CastSpellButton => castSpellButton;

        public void TriggerCastSpellButtonHandler()
        {
            OnCastSpellButtonClick?.Invoke();
        }
    }
}