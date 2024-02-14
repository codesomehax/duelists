using FishNet.Broadcast;

namespace Network.Authentication
{
    public struct BasicCredentialsBroadcast : IBroadcast
    {
        public string Username; 
        public string Password;
    }
}