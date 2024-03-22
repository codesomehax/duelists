using FishNet.Broadcast;

namespace Network.Authentication
{
    public struct AuthenticationFailedMessageBroadcast : IBroadcast
    {
        public string Message;
    }
}