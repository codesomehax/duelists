using System.Collections.Generic;
using System.Linq;
using Factions;
using UI.Menu;
using Units;
using UnityEngine;

namespace UI.Lobby.Settings
{
    public class LobbySettingsMenu : MonoBehaviour
    {
        [SerializeField] private LanMenu lanMenu;

        private FactionSettingsPanel[] _factionSettingsList;
        private FactionSettingsPanel[] FactionSettingsList => _factionSettingsList ??= 
            FindObjectsByType<FactionSettingsPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        public IDictionary<UnitType, UnitSetting> GetSettingsByFaction(Faction faction)
        {
            return FactionSettingsList
                .First(factionSettings => factionSettings.Faction == faction)
                .GetAllSettings();
        }

        public void GoBack()
        {
            gameObject.SetActive(false);
            lanMenu.gameObject.SetActive(true);
        }
    }
}