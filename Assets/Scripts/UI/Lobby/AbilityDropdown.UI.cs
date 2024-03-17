using System.Collections.Generic;
using TMPro;
using Units.Hero;
using UnityEngine;

namespace UI.Lobby
{
    public partial class AbilityDropdown
    {
        private static readonly IDictionary<string, AbilityType> Abilities = new Dictionary<string, AbilityType>
        {
            { "Strength", AbilityType.StrengthIncrease },
            { "Agility", AbilityType.AgilityIncrease },
            { "Intelligence", AbilityType.IntelligenceIncrease }
        };
        
        [SerializeField] private TMP_Dropdown abilityTypeDropdown;

        public void TriggerOnValueChanged()
        {
            if (!IsOwner) return;
            
            string dropdownValue = abilityTypeDropdown.options[abilityTypeDropdown.value].text;
            if (Abilities.TryGetValue(dropdownValue, out AbilityType abilityType))
                SetAbilityTypeServerRpc(abilityType);
            else
                Debug.Log($"Unknown ability type {dropdownValue} received from Ability Type Dropdown");
        }

        private void SyncAbilityType(AbilityType prev, AbilityType next, bool asServer)
        {
            if (asServer) return;
            foreach (KeyValuePair<string, AbilityType> pair in Abilities)
            {
                if (pair.Value != next) continue;

                string key = pair.Key;
                int value = abilityTypeDropdown.options.FindIndex(optionData => optionData.text == key);
                abilityTypeDropdown.value = value;
            }
        }
    }
}