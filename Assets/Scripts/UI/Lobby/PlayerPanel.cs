using System;
using System.Collections.Generic;
using Factions;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UI.Lobby.Settings;
using Units;
using UnityEngine;

namespace UI.Lobby
{
    public partial class PlayerPanel : NetworkBehaviour
    {
        [SerializeField] private FactionDropdown factionDropdown;

        [SyncVar(OnChange = nameof(SetUsernameText))]
        [NonSerialized]
        public string Username;

        private const string LeftPlayerPanelContainerName = "Left Player Panel Container";
        private const string RightPlayerPanelContainerName = "Right Player Panel Container";

        private UnitInputPanel[] _unitInputPanels;

        private LobbySettingsMenu _lobbySettingsMenu;

        private void Awake()
        {
            _lobbySettingsMenu = FindObjectOfType<LobbySettingsMenu>(true);
            _unitInputPanels = GetComponentsInChildren<UnitInputPanel>();
            factionDropdown.OnFactionChanged += AdjustUnitInputsToFaction;
        }

        public override void OnStartServer()
        {
            AdjustUnitInputsToFaction(Faction.Heaven);
        }

        public override void OnStartClient()
        {
            SetupParentLocally();
        }

        private void SetupParentLocally()
        {
            string containerName = !(IsHost ^ ClientManager.Connection.ClientId == OwnerId)
                ? LeftPlayerPanelContainerName
                : RightPlayerPanelContainerName;
            Transform parent = GameObject.Find(containerName).transform;
            transform.SetParent(parent, false);
        }
        
        [Server]
        private void AdjustUnitInputsToFaction(Faction faction)
        {
            IDictionary<UnitType, UnitSetting> defaultSettings = _lobbySettingsMenu.GetSettingsByFaction(faction);
            foreach (UnitInputPanel unitInputPanel in _unitInputPanels)
            {
                UnitSetting unitSetting = defaultSettings[unitInputPanel.UnitType];
                unitInputPanel.UnitName = unitSetting.UnitName;
                unitInputPanel.MaxCount = unitSetting.MaxCount;
            }
        }
    }
}