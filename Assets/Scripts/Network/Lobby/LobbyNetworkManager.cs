using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;

namespace Network.Lobby
{
    public class LobbyNetworkManager : NetworkBehaviour
    {
        [SyncVar] [NonSerialized] public string Username1;
        [SyncVar] [NonSerialized] public string Username2;
    }
}