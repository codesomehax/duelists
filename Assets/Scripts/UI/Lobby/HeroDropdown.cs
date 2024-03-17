using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units.Hero;

namespace UI.Lobby
{
    public partial class HeroDropdown : NetworkBehaviour
    {
        [SyncVar(OnChange = nameof(SyncHeroType))] [NonSerialized] public HeroType HeroType;
        
        [ServerRpc]
        private void SetHeroServerRpc(HeroType heroType)
        {
            HeroType = heroType;
        }

        public override void OnStartClient()
        {
            if (!IsOwner)
                heroDropdown.enabled = false;
        }
    }
}