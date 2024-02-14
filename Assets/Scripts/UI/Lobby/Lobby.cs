using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace UI.Lobby
{
    [RequireComponent(typeof(LobbyUIService))]
    public class Lobby : NetworkBehaviour
    {
        [SerializeField] private PlayerPanel playerPanelPrefab;
        
        public string Username1 { get; set; }
        public string Username2 { get; set; }

        private LobbyUIService _lobbyUIService;

        private const string BackgroundObjectName = "Background";

        private void Awake()
        {
            _lobbyUIService = GetComponent<LobbyUIService>();
        }

        public override void OnStartServer()
        {
            SceneManager.OnClientLoadedStartScenes += SpawnPlayerPanel;
        }

        private void SpawnPlayerPanel(NetworkConnection connection, bool asServer)
        {
            if (!asServer) return;
            
            PlayerPanel playerPanel = Instantiate(playerPanelPrefab);

            string username = connection.IsHost ? Username1 : Username2;
            playerPanel.Username = username;
            
            Spawn(playerPanel.NetworkObject, connection);
            SceneManager.AddOwnerToDefaultScene(playerPanel.NetworkObject);
        }

        public override void OnStartClient()
        {
            SetupParentLocally();
        }

        private void SetupParentLocally()
        {
            Transform backgroundTransform = GameObject.Find(BackgroundObjectName).transform;
            transform.SetParent(backgroundTransform, false);
        }
    }
}