using System.Collections.Generic;
using Battle.UI;
using Units;

namespace Battle.Player
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
            _playerUI.EndTurnButton.onClick.AddListener(EndTurnButtonHandler);
            _playerUI.OnCastSpellButtonClick += CastSpell;
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
            _gridManager.SetAllToPlaceholder();
            SetPlayerReadyServerRpc();
        }

        private void ShowEndTurnButton()
        {
            _playerUI.EndTurnButton.gameObject.SetActive(true);
        }

        private void EndTurnButtonHandler()
        {
            _playerUI.EndTurnButton.gameObject.SetActive(false);
            UnmarkReachablePositions();
            UnmarkPositionsInAttackRange();
            HideCastSpellButton();
            EndTurnServerRpc();
        }

        private void ShowCastSpellButton()
        {
            _playerUI.CastSpellButton.gameObject.SetActive(true);
        }

        private void HideCastSpellButton()
        {
            _playerUI.CastSpellButton.gameObject.SetActive(false);
        }

        private void CastSpell()
        {
            HideCastSpellButton();
            ActingUnit.TryCastSpell();
        }
    }
}