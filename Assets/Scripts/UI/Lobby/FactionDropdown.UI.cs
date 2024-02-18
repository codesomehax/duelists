using System;
using Factions;
using FishNet.Object;
using TMPro;
using UnityEngine;

namespace UI.Lobby
{
    public partial class FactionDropdown
    {
        [SerializeField] private TMP_Dropdown factionDropdown;

        public void TriggerOnValueChanged()
        {
            if (!IsOwner) return;
            
            string dropdownValue = factionDropdown.options[factionDropdown.value].text;
            if (Enum.TryParse(dropdownValue, out Faction faction))
                SetFactionServerRpc(faction);
            else
                Debug.Log($"Unknown faction {dropdownValue} received from Faction Dropdown");
        }

        private void UpdateDropdownUI(Faction prev, Faction next, bool asServer)
        {
            if (asServer) return;
            int value = factionDropdown.options.FindIndex(optionData => optionData.text == next.ToString());
            factionDropdown.value = value;
        }
    }
}