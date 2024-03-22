using System;
using FishNet.Authenticating;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Transporting;
using UI;
using UnityEngine;

namespace Network.Authentication
{
    public class BasicAuthenticator : Authenticator
    {
        private const int MaxPlayerCount = 2;
        
        public override event Action<NetworkConnection, bool> OnAuthenticationResult;

        public string Username1 { get; private set; }
        public string Username2 { get; private set; }
        public string Password { private get; set; }
        public int PlayerCount { get; set; }

        [SerializeField] private PopupManager popupManager;

        public override void InitializeOnce(NetworkManager networkManager)
        {
            base.InitializeOnce(networkManager);

            PlayerCount = 0;
            
            networkManager.ServerManager
                .RegisterBroadcast<BasicCredentialsBroadcast>(OnServerCredentialsBroadcast, false);
            networkManager.ServerManager.OnServerConnectionState += ResetSettings;
            networkManager.ServerManager.OnRemoteConnectionState += DecreasePlayerCount;
            networkManager.ClientManager
                .RegisterBroadcast<AuthenticationFailedMessageBroadcast>(OnAuthenticationFailedBroadcast);
        }

        private void ResetSettings(ServerConnectionStateArgs stateArgs)
        {
            if (stateArgs.ConnectionState == LocalConnectionState.Stopped)
            {
                Username1 = Username2 = Password = null;
                PlayerCount = 0;
            }
        }

        private void DecreasePlayerCount(NetworkConnection connection, RemoteConnectionStateArgs stateArgs)
        {
            if (stateArgs.ConnectionState == RemoteConnectionState.Stopped && connection.Authenticated)
            {
                PlayerCount--;
                Username2 = null;
            }
        }

        public override void OnRemoteConnection(NetworkConnection connection)
        {
            if (PlayerCount == 2)
            {
                AuthenticationFailedMessageBroadcast broadcast = new() { Message = "The lobby is full" };
                NetworkManager.ServerManager.Broadcast(broadcast);
                connection.Disconnect(false);
            }
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
            bool isUsernameBusy = basicCredentialsBroadcast.Username == Username1
                                  || basicCredentialsBroadcast.Username == Username2;
            bool authenticated = isPasswordCorrect && !isMaxPlayersCountReached && !isUsernameBusy;
            
            if (authenticated)
            {
                PlayerCount++;
                if (PlayerCount == 1)
                    Username1 = basicCredentialsBroadcast.Username;
                else
                    Username2 = basicCredentialsBroadcast.Username;
            }
            else
            {
                AuthenticationFailedMessageBroadcast broadcast = new() { Message = "Authentication failed" };
                NetworkManager.ServerManager.Broadcast(connection, broadcast, false);
            }
            
            OnAuthenticationResult?.Invoke(connection, authenticated);
        }

        private void OnAuthenticationFailedBroadcast(AuthenticationFailedMessageBroadcast broadcast)
        {
            popupManager.ShowPopupWithMessage(broadcast.Message);
        }
    }
}