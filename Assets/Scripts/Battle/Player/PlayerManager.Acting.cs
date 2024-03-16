using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using Units;
using Units.Battle;
using UnityEngine;

namespace Battle.Player
{
    public partial class PlayerManager
    {
        private IDictionary<Vector3Int, IList<Vector3Int>> _reachablePositions;

        private void StartTurn(PlayerState prev, PlayerState next, bool asServer)
        {
            if (asServer && next == PlayerState.Acting)
            {
                ActingUnit.OnDestinationReached += SetPlayerMoved;
                ActingUnit.OnDestinationReached += MarkPositionsInAttackRangeTargetRpc;
                return;
            }
            
            if (asServer || !IsOwner || next != PlayerState.Acting) return;

            _reachablePositions = BattleUnitPathfinder.GetReachablePositions(ActingUnit);
            _gridManager.MarkPositionsAs(_reachablePositions.Keys, ActionTileState.Available);
            MarkPositionsInAttackRange();
        }

        private void SetPlayerMoved(NetworkConnection connection)
        {
            _playerMoved = true;
        }

        [TargetRpc]
        private void MarkPositionsInAttackRangeTargetRpc(NetworkConnection connection)
        {
            MarkPositionsInAttackRange();
        }

        private void MarkPositionsInAttackRange()
        {
            ISet<Vector3Int> positionsInAttackRange = BattleUnitPathfinder.GetUnitsInAttackRangeFor(ActingUnit);
            _gridManager.MarkPositionsAs(positionsInAttackRange, ActionTileState.Attack);
        }

        private void UnmarkPositionsInAttackRange()
        {
            ISet<Vector3Int> positionsInAttackRange = BattleUnitPathfinder.GetUnitsInAttackRangeFor(ActingUnit);
            _gridManager.MarkPositionsAs(positionsInAttackRange, ActionTileState.Placeholder);
        }

        [ServerRpc]
        private void MoveActingUnitToPositionServerRpc(Vector3Int position)
        {
            if (_playerMoved) return;
            
            _reachablePositions ??= BattleUnitPathfinder.GetReachablePositions(ActingUnit);
            if (_reachablePositions.TryGetValue(position, out IList<Vector3Int> tilePath))
            {
                UnmarkMovementAndAttackPositionsTargetRpc(Owner);
                ActingUnit.CellPosition = position;
                IList<Vector3> worldPath = tilePath.Select(tile => _gridManager.CellPositionToWorld(tile)).ToList();
                ActingUnit.FollowPath(worldPath);
            }
        }

        [TargetRpc]
        private void UnmarkMovementAndAttackPositionsTargetRpc(NetworkConnection connection)
        {
            UnmarkPositionsInAttackRange();
            _gridManager.MarkPositionsAs(_reachablePositions.Keys, ActionTileState.Placeholder);
            _reachablePositions = null;
        }

        [ServerRpc]
        private void AttackUnitAtPositionServerRpc(Vector3Int position)
        {
            Vector3Int distanceVector = position - ActingUnit.CellPosition;
            int distance = Math.Abs(distanceVector.x) + Math.Abs(distanceVector.y);
            if (distance > ActingUnit.AttackRange) return;

            BattleUnit enemyUnit = EnemyUnitsCollection.FirstOrDefault(unit => unit.CellPosition == position);
            if (enemyUnit == null) return;
            
            // TODO math logic etc

            Vector3 enemyUnitWorldPosition = _gridManager.CellPositionToWorld(enemyUnit.CellPosition);
            AnimationType animationType = distance == 1 ? AnimationType.AttackMelee : AnimationType.AttackRanged;
            ActingUnit.AttackUnitAtPosition(enemyUnitWorldPosition, animationType);
        }
    }
}