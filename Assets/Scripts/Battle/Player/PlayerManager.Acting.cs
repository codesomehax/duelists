using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object.Synchronizing;
using Units.Battle;
using UnityEngine;
using Utils;

namespace Battle.Player
{
    public partial class PlayerManager
    {
        [SyncVar] [NonSerialized] public BattleUnit ActingUnit;
        private IDictionary<Vector3Int, List<Vector3Int>> _reachablePositions;

        private void StartTurn(PlayerState prev, PlayerState next, bool asServer)
        {
            if (asServer || !IsOwner || next != PlayerState.Acting) return;

            _reachablePositions = BattleUnitPathfinder.GetReachablePositions(ActingUnit);
            _gridManager.MarkPositionsAs(_reachablePositions.Keys, ActionTileState.Available);
        }
    }
}