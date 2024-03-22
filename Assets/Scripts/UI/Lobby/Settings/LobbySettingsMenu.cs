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
        [SerializeField] private PlayMenu playMenu;
        [SerializeField] private GeneralSettingsPanel generalSettingsPanel;
        [SerializeField] private List<FactionSettingsPanel> factionSettingsList;

        public int MaxGold => generalSettingsPanel.Gold;

        public IDictionary<UnitType, UnitSetting> GetSettingsByFaction(Faction faction)
        {
            return factionSettingsList
                .First(factionSettings => factionSettings.Faction == faction)
                .GetAllSettings();
        }

        public void GoBack()
        {
            gameObject.SetActive(false);
            playMenu.gameObject.SetActive(true);
        }
    }
}