using FishNet;
using FishNet.Transporting;
using UnityEngine;

namespace Network.Lobby
{
    public class LobbyNetworkService : MonoBehaviour
    {
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
                BasicAuthenticator authenticator = FindObjectOfType<BasicAuthenticator>();
                authenticator.Password = _password;

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
            if (username.Length is 0 or > MaxUsernameLength) return;
            if (password.Length is 0 or > MaxPasswordLength) return;
            
            _username = username;
            _password = password;

            InstanceFinder.ClientManager.StartConnection(ipAddress);
        }
    }
}