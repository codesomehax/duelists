using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace UI.Lobby
{
    [RequireComponent(typeof(PlayerPanelUIService))]
    public class PlayerPanel : NetworkBehaviour
    {
        [SyncVar(OnChange = nameof(SetUsernameText))]
        [NonSerialized]
        public string Username;

        private PlayerPanelUIService _playerPanelUIService;

        private const string LeftPlayerPanelContainerName = "Left Player Panel Container";
        private const string RightPlayerPanelContainerName = "Right Player Panel Container";

        private void Awake()
        {
            _playerPanelUIService = GetComponent<PlayerPanelUIService>();
        }

        private void SetUsernameText(string prev, string next, bool asServer)
        {
            if (asServer) return;
            _playerPanelUIService.Username = next;
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
    }
}