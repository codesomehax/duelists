using FishNet;
using FishNet.Connection;
using FishNet.Transporting;
using Network.Authentication;
using UI;
using UI.Lobby;
using UnityEngine;

namespace Network
{
    public class NetworkSetupManager : MonoBehaviour
    {
        [SerializeField] private PopupManager popupManager;
        [SerializeField] private Lobby lobbyPrefab;
        [SerializeField] private PlayerPanel playerPanelPrefab;
        
        private string _username;
        private string _password;

        private const int MaxUsernameLength = 32;
        private const int MaxPasswordLength = 32;

        private BasicAuthenticator _authenticator;

        private void Awake()
        {
            _authenticator = InstanceFinder.ServerManager.GetAuthenticator() as BasicAuthenticator;
            InstanceFinder.ServerManager.OnServerConnectionState += SetupAuthenticatorAndConnectAsHost;
            InstanceFinder.ClientManager.OnClientConnectionState += Authenticate;
            InstanceFinder.SceneManager.OnClientLoadedStartScenes += SpawnPlayerPanel;
        }
        
        public void HostGame(string username, string password)
        {
            if (!ValidateUsernameAndPassword(username, password)) return;
            
            _username = username;
            _password = password;

            InstanceFinder.ServerManager.StartConnection();
        }

        private bool ValidateUsernameAndPassword(string username, string password)
        {
            if (username.Length == 0)
            {
                popupManager.ShowPopupWithMessage("Username cannot be empty.");
                return false;
            }
            if (username.Length > MaxUsernameLength)
            {
                popupManager.ShowPopupWithMessage($"Username cannot be longer than {MaxUsernameLength} characters.");
                return false;
            }
            
            if (password.Length == 0)
            {
                popupManager.ShowPopupWithMessage("Password cannot be empty.");
                return false;
            }
            if (username.Length > MaxPasswordLength)
            {
                popupManager.ShowPopupWithMessage($"Password cannot be longer than {MaxPasswordLength} characters.");
                return false;
            }

            return true;
        }

        private void SetupAuthenticatorAndConnectAsHost(ServerConnectionStateArgs stateArgs)
        {
            if (stateArgs.ConnectionState == LocalConnectionState.Started)
            {
                Lobby lobby = Instantiate(lobbyPrefab);
                InstanceFinder.ServerManager.Spawn(lobby.NetworkObject);

                _authenticator.PlayerCount = 0;
                _authenticator.Password = _password;
            
                InstanceFinder.ClientManager.StartConnection();
            }
        }
        
        private void Authenticate(ClientConnectionStateArgs stateArgs)
        {
            if (stateArgs.ConnectionState == LocalConnectionState.Started)
            {
                BasicCredentialsBroadcast basicCredentialsBroadcast = new BasicCredentialsBroadcast
                {
                    Username = _username,
                    Password = _password
                };
                InstanceFinder.ClientManager.Broadcast(basicCredentialsBroadcast);
            }
        }

        public void JoinGame(string username, string ipAddress, string password)
        {
            if (!ValidateUsernameAndPassword(username, password)) return;
            
            _username = username;
            _password = password;

            InstanceFinder.ClientManager.StartConnection(ipAddress);
        }
        
        private void SpawnPlayerPanel(NetworkConnection connection, bool asServer)
        {
            if (!asServer) return;
            
            PlayerPanel playerPanel = Instantiate(playerPanelPrefab);

            string username = connection.IsHost ? _authenticator.Username1 : _authenticator.Username2;
            playerPanel.Username = username;
            
            InstanceFinder.ServerManager.Spawn(playerPanel.NetworkObject, connection);
            InstanceFinder.SceneManager.AddOwnerToDefaultScene(playerPanel.NetworkObject);
        }

        public void LeaveAsHost()
        {
            LeaveAsClient();
            InstanceFinder.ServerManager.StopConnection(true);
        }

        public void LeaveAsClient()
        {
            InstanceFinder.ClientManager.StopConnection();
        }

        private void OnDestroy()
        {
            InstanceFinder.ServerManager.OnServerConnectionState -= SetupAuthenticatorAndConnectAsHost;
            InstanceFinder.ClientManager.OnClientConnectionState -= Authenticate;
            InstanceFinder.SceneManager.OnClientLoadedStartScenes -= SpawnPlayerPanel;
        }
    }
}