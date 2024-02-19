using System;
using FishNet.Connection;
using FishNet.Object;
using Network;
using UI.Menu;
using UnityEngine;

namespace UI.Lobby
{
    public class Lobby : NetworkBehaviour
    {
        private const string BackgroundObjectName = "Background";

        private LanMenu _lanMenu;
        private NetworkSetupManager _networkSetupManager;

        private void Awake()
        {
            _lanMenu = FindObjectOfType<LanMenu>(true);
            _networkSetupManager = FindObjectOfType<NetworkSetupManager>();
        }

        public override void OnStartClient()
        {
            Debug.Log($"is null {_lanMenu} and {_networkSetupManager}");
            SetupParentLocally();
        }

        private void SetupParentLocally()
        {
            Transform backgroundTransform = GameObject.Find(BackgroundObjectName).transform;
            transform.SetParent(backgroundTransform, false);
        }

        public void LeaveLobby()
        {
            if (IsServer)
                _networkSetupManager.LeaveAsHost();
            else
                _networkSetupManager.LeaveAsClient();
        }

        // public override void OnStopServer()
        // {
        //     SceneManager.OnClientLoadedStartScenes -= SpawnPlayerPanel;
        // }

        public override void OnStopClient()
        {
            gameObject.SetActive(false);
            _lanMenu.gameObject.SetActive(true);
        }
    }
}