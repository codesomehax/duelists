using System;
using FishNet.Authenticating;
using FishNet.Connection;
using FishNet.Managing;

namespace Network.Lobby
{
    public class BasicAuthenticator : Authenticator
    {
        private const int MaxPlayerCount = 2;
        
        public override event Action<NetworkConnection, bool> OnAuthenticationResult;

        public string Password { private get; set; }
        private int PlayerCount { get; set; }

        private LobbyNetworkManager _lobbyNetworkManager;
        private LobbyNetworkManager LobbyNetworkManager
        {
            get
            {
                if (_lobbyNetworkManager == null)
                    _lobbyNetworkManager = FindObjectOfType<LobbyNetworkManager>();
                return _lobbyNetworkManager;
            }
        }

        public override void InitializeOnce(NetworkManager networkManager)
        {
            base.InitializeOnce(networkManager);

            PlayerCount = 0;
            
            networkManager.ServerManager
                .RegisterBroadcast<BasicCredentialsBroadcast>(OnServerCredentialsBroadcast, false);
        }

        private void OnServerCredentialsBroadcast(
            NetworkConnection connection, 
            BasicCredentialsBroadcast basicCredentialsBroadcast)
        {
            if (connection.Authenticated)
            {
                connection.Disconnect(true);
                return;
            }
            
            bool isPasswordCorrect = basicCredentialsBroadcast.Password == Password;
            bool isMaxPlayersCountExceeded = PlayerCount >= MaxPlayerCount;
            bool isUsernameBusy = basicCredentialsBroadcast.Username == LobbyNetworkManager.Username1
                                  || basicCredentialsBroadcast.Username == LobbyNetworkManager.Username2;
            bool authenticated = isPasswordCorrect && !isMaxPlayersCountExceeded && !isUsernameBusy;
            
            if (authenticated)
            {
                PlayerCount++;
                if (PlayerCount == 1)
                    LobbyNetworkManager.Username1 = basicCredentialsBroadcast.Username;
                else
                    LobbyNetworkManager.Username2 = basicCredentialsBroadcast.Username;
            }
            
            OnAuthenticationResult?.Invoke(connection, authenticated);
        }
    }
}