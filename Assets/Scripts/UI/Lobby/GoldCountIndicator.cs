using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace UI.Lobby
{
    public partial class GoldCountIndicator : NetworkBehaviour
    {
        [SyncVar(OnChange = nameof(SyncGoldCount))] [NonSerialized] 
        public int MaxGold;
        [SyncVar(OnChange = nameof(SyncGoldCount))] [NonSerialized]
        public int AvailableGold;
    }
}