using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using Units.Battle;
using UnityEngine;

namespace Battle.Player
{
    public partial class PlayerManager
    {
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
                UnmarkMovementPositionsTargetRpc(Owner);
                ActingUnit.CellPosition = position;
                IList<Vector3> worldPath = tilePath.Select(tile => _gridManager.CellPositionToWorld(tile)).ToList();
                ActingUnit.FollowPath(worldPath);
            }
        }

        [TargetRpc]
        private void UnmarkMovementPositionsTargetRpc(NetworkConnection connection)
        {
            _gridManager.MarkPositionsAs(_reachablePositions.Keys, ActionTileState.Placeholder);
            _reachablePositions = null;
        }
    }
}