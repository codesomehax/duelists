using System;
using TMPro;
using Units.Hero;
using UnityEngine;

namespace UI.Lobby
{
    public partial class HeroDropdown
    {
        [SerializeField] private TMP_Dropdown heroDropdown;

        public void TriggerOnValueChanged()
        {
            if (!IsOwner) return;
            
            string dropdownValue = heroDropdown.options[heroDropdown.value].text;
            if (Enum.TryParse(dropdownValue, out HeroType heroType))
                SetHeroServerRpc(heroType);
            else
                Debug.Log($"Unknown hero type {dropdownValue} received from Hero Dropdown");
        }

        private void SyncHeroType(HeroType prev, HeroType next, bool asServer)
        {
            if (asServer) return;
            int value = heroDropdown.options.FindIndex(optionData => optionData.text == next.ToString());
            heroDropdown.value = value;
        }
    }
}