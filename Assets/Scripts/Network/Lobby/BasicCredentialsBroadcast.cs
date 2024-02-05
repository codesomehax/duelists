using FishNet.Broadcast;

namespace Network.Lobby
{
    public struct BasicCredentialsBroadcast : IBroadcast
    {
        public string Username; 
        public string Password;
    }
}