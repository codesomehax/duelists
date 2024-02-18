using FishNet;
using FishNet.Transporting;
using Network.Authentication;
using UI.Lobby;
using UnityEngine;

namespace Network
{
    public class NetworkSetupManager : MonoBehaviour
    {
        [SerializeField] private Lobby lobbyPrefab;
        
        private string _username;
        private string _password;

        private const int MaxUsernameLength = 32;
        private const int MaxPasswordLength = 32;

        private void Awake()
        {
            InstanceFinder.ServerManager.OnServerConnectionState += SetupAuthenticatorAndConnectAsHost;
            InstanceFinder.ClientManager.OnClientConnectionState += Authenticate;
        }
        
        public void HostGame(string username, string password)
        {
            if (username.Length is 0 or > MaxUsernameLength) return;
            if (password.Length is 0 or > MaxPasswordLength) return;
            
            _username = username;
            _password = password;

            InstanceFinder.ServerManager.StartConnection();
        }

        private void SetupAuthenticatorAndConnectAsHost(ServerConnectionStateArgs stateArgs)
        {
            if (stateArgs.ConnectionState == LocalConnectionState.Started)
            {
                Lobby lobby = Instantiate(lobbyPrefab);
                InstanceFinder.ServerManager.Spawn(lobby.NetworkObject);

                BasicAuthenticator authenticator = InstanceFinder.ServerManager.GetAuthenticator() as BasicAuthenticator;
                if (authenticator != null)
                {
                    authenticator.Password = _password;
                    authenticator.Lobby = lobby;
                
                    InstanceFinder.ClientManager.StartConnection();
                }
                else
                {
                    Debug.Log("Authenticator is missing"); // TODO change behavior
                }
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
            if (username.Length is 0 or > MaxUsernameLength) return;
            if (password.Length is 0 or > MaxPasswordLength) return;
            
            _username = username;
            _password = password;

            InstanceFinder.ClientManager.StartConnection(ipAddress);
        }
    }
}