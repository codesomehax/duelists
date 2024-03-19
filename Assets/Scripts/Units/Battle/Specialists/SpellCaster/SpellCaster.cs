using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Units.Battle.Specialists.SpellCaster
{
    [RequireComponent(typeof(BattleUnit))]
    public class SpellCaster : NetworkBehaviour
    {
        [SyncVar] [NonSerialized] public bool IsSpellCasted;
        
        [SerializeField] private Spell spell;

        private BattleUnit _battleUnit;
        
        private void Awake()
        {
            _battleUnit = GetComponent<BattleUnit>();
        }

        [ServerRpc]
        public void CastServerRpc()
        {
            if (!IsSpellCasted)
            {
                spell.Cast(_battleUnit);
                IsSpellCasted = true;
            }
        }
    }
}