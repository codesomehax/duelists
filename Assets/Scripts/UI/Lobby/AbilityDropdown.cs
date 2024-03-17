using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units.Hero;
using UnityEngine;

namespace UI.Lobby
{
    public partial class AbilityDropdown : NetworkBehaviour
    {
        [SyncVar(OnChange = nameof(SyncAbilityType))] [NonSerialized] public AbilityType AbilityType;
        
        [ServerRpc]
        private void SetAbilityTypeServerRpc(AbilityType abilityType)
        {
            AbilityType = abilityType;
            Debug.Log($"ServerRpc parameter {abilityType} syncvar {AbilityType}");
        }

        public override void OnStartClient()
        {
            if (!IsOwner)
                abilityTypeDropdown.enabled = false;
        }
    }
}