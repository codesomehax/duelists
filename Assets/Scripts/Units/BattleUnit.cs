using System;
using Factions;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Units
{
    public class BattleUnit : NetworkBehaviour
    {
        public static event Action<Vector3Int> OnUnitPlaced;
        public static event Action<Vector3Int> OnUnitRemoval;

        [SyncVar] [NonSerialized] public int Count;
        [SyncVar] [NonSerialized] public Vector3Int CellPosition;

        public string Name => battleUnitInfo.Name;
        public Faction Faction => battleUnitInfo.Faction;
        public UnitType UnitType => battleUnitInfo.UnitType;
        
        [SerializeField] private BattleUnitInfo battleUnitInfo;

        public override void OnStartClient()
        {
            OnUnitPlaced?.Invoke(CellPosition);
        }

        public override void OnStopClient()
        {
            OnUnitRemoval?.Invoke(CellPosition);
        }
    }
}