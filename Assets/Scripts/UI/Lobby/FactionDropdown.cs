using System;
using Factions;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace UI.Lobby
{
    public partial class FactionDropdown : NetworkBehaviour
    {
        [SyncVar(OnChange = nameof(UpdateDropdownUI))]
        private Faction _faction = Faction.Heaven;

        public event Action<Faction> OnFactionChangedServerside;

        [ServerRpc]
        private void SetFactionServerRpc(Faction faction)
        {
            _faction = faction;
            OnFactionChangedServerside?.Invoke(faction);
        }

        public override void OnStartClient()
        {
            if (!IsOwner)
                factionDropdown.enabled = false;
        }
    }
}