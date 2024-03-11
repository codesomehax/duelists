using System.Collections.Generic;
using Battle.UI;
using Units;

namespace Battle
{
    public partial class PlayerManager
    {
        private PlayerUIComponents _playerUI;

        private void SetupUI()
        {
            _playerUI = FindObjectOfType<PlayerUIComponents>();
            _playerUI.PlaceUnitWindow.OnUnitPlaced += PlaceUnit;
            _playerUI.UnitRemovalConfirmationWindow.OnRemovalConfirmed += RemoveUnit;
            _playerUI.ReadyButton.onClick.AddListener(ReadyButtonHandler);
        }

        private void ShowPlaceUnitWindow(IDictionary<UnitType, PlaceUnitData> placeUnitData)
        {
            _playerUI.PlaceUnitWindow.Activate(placeUnitData);
        }

        private void AskRemoveUnitWindow()
        {
            _playerUI.UnitRemovalConfirmationWindow.gameObject.SetActive(true);
        }

        private void ReadyButtonHandler()
        {
            _playerUI.ReadyButton.gameObject.SetActive(false);
            SetPlayerReadyServerRpc();
        }
    }
}