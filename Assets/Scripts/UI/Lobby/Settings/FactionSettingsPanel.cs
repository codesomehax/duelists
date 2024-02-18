using System.Collections.Generic;
using System.Linq;
using Factions;
using Units;
using UnityEngine;

namespace UI.Lobby.Settings
{
    public class FactionSettingsPanel : MonoBehaviour
    {
        [SerializeField] private Faction faction;

        public Faction Faction => faction;
        
        private UnitSettingPanel[] _unitSettingPanels;
        private UnitSettingPanel[] UnitSettingPanels => _unitSettingPanels ??=
            GetComponentsInChildren<UnitSettingPanel>(true);

        public IDictionary<UnitType, UnitSetting> GetAllSettings()
        {
            return UnitSettingPanels.ToDictionary(
                unitSettingPanel => unitSettingPanel.UnitType,
                unitSettingPanel => unitSettingPanel.UnitSetting);
        }
    }
}