using System;
using Factions;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace UI.Lobby
{
    public partial class FactionDropdown : NetworkBehaviour
    {
        [field: SyncVar(OnChange = nameof(UpdateDropdownUI))]
        public Faction Faction { get; private set; }

        public event Action<Faction> OnFactionChangedServerside;

        [ServerRpc]
        private void SetFactionServerRpc(Faction faction)
        {
            Faction = faction;
            OnFactionChangedServerside?.Invoke(faction);
        }

        public override void OnStartClient()
        {
            if (!IsOwner)
                factionDropdown.enabled = false;
        }
    }
}