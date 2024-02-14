using System;
using FishNet.Authenticating;
using FishNet.Connection;
using FishNet.Managing;

namespace Network.Authentication
{
    public class BasicAuthenticator : Authenticator
    {
        private const int MaxPlayerCount = 2;
        
        public override event Action<NetworkConnection, bool> OnAuthenticationResult;

        public string Password { private get; set; }
        public UI.Lobby.Lobby Lobby { private get; set; }
        private int PlayerCount { get; set; } // TODO find a way to decrease this value on disconnection

        public override void InitializeOnce(NetworkManager networkManager)
        {
            base.InitializeOnce(networkManager);

            PlayerCount = 0;
            
            networkManager.ServerManager
                .RegisterBroadcast<BasicCredentialsBroadcast>(OnServerCredentialsBroadcast, false);
        }

        public override void OnRemoteConnection(NetworkConnection connection)
        {
            if (PlayerCount == 2)
                // TODO send some reason message
                connection.Disconnect(true);
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
            bool isMaxPlayersCountReached = PlayerCount == MaxPlayerCount;
            bool isUsernameBusy = basicCredentialsBroadcast.Username == Lobby.Username1
                                  || basicCredentialsBroadcast.Username == Lobby.Username2;
            bool authenticated = isPasswordCorrect && !isMaxPlayersCountReached && !isUsernameBusy;
            
            if (authenticated)
            {
                PlayerCount++;
                if (PlayerCount == 1)
                    Lobby.Username1 = basicCredentialsBroadcast.Username;
                else
                    Lobby.Username2 = basicCredentialsBroadcast.Username;
            }
            
            OnAuthenticationResult?.Invoke(connection, authenticated);
        }
    }
}