using UnityEngine;

namespace UI.Lobby.Settings
{
    public class GeneralSettingsPanel : MonoBehaviour
    {
        [SerializeField] private GoldSettingPanel goldSettingPanel;

        public int Gold => goldSettingPanel.Gold;
    }
}