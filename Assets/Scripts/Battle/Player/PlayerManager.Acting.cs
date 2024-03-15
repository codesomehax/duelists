using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units.Battle;
using UnityEngine;

namespace Battle.Player
{
    public partial class PlayerManager
    {
        [SyncVar] [NonSerialized] public BattleUnit ActingUnit;
        
        private IDictionary<Vector3Int, IList<Vector3Int>> _reachablePositions;

        private void StartTurn(PlayerState prev, PlayerState next, bool asServer)
        {
            if (asServer || !IsOwner || next != PlayerState.Acting) return;

            _reachablePositions = BattleUnitPathfinder.GetReachablePositions(ActingUnit);
            _gridManager.MarkPositionsAs(_reachablePositions.Keys, ActionTileState.Available);
        }

        [ServerRpc]
        private void MoveActingUnitToPositionServerRpc(Vector3Int position)
        {
            _reachablePositions ??= BattleUnitPathfinder.GetReachablePositions(ActingUnit);

            if (_reachablePositions.TryGetValue(position, out IList<Vector3Int> tilePath))
            {
                ActingUnit.CellPosition = position;
                IList<Vector3> worldPath = tilePath.Select(tile => _gridManager.CellPositionToWorld(tile)).ToList();
                ActingUnit.FollowPath(worldPath);

                _reachablePositions = null; // TODO only if moved!
            }
        }
    }
}