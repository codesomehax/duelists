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
        public event Action<PlayerManager> OnTurnEnded;
        
        private IDictionary<Vector3Int, IList<Vector3Int>> _reachablePositions;
        private ISet<Vector3Int> _positionsInAttackRange;

        private void StartTurn(PlayerState prev, PlayerState next, bool asServer)
        {
            if (asServer && next == PlayerState.Acting)
            {
                ActingUnit.OnDestinationReached += SetPlayerMoved;
                ActingUnit.OnDestinationReached += MarkPositionsInAttackRangeTargetRpc;
                return;
            }
            
            if (asServer || !IsOwner || next != PlayerState.Acting) return;

            MarkReachablePositions();
            MarkPositionsInAttackRange();
            ShowEndTurnButton();
        }

        private void SetPlayerMoved(NetworkConnection connection)
        {
            _playerMoved = true;
        }

        private void MarkReachablePositions()
        {
            _reachablePositions = BattleUnitPathfinder.GetReachablePositions(ActingUnit);
            _gridManager.MarkPositionsAs(_reachablePositions.Keys, ActionTileState.Available);
        }

        private void UnmarkReachablePositions()
        {
            if (_reachablePositions == null) return;
            
            _gridManager.MarkPositionsAs(_reachablePositions.Keys, ActionTileState.Placeholder);
            _reachablePositions = null;
        }

        private void MarkPositionsInAttackRange()
        {
            _positionsInAttackRange = BattleUnitPathfinder.GetUnitsInAttackRangeFor(ActingUnit);
            _gridManager.MarkPositionsAs(_positionsInAttackRange, ActionTileState.Attack);
        }

        private void UnmarkPositionsInAttackRange()
        {
            _gridManager.MarkPositionsAs(_positionsInAttackRange, ActionTileState.Placeholder);
            _positionsInAttackRange = null;
        }

        [TargetRpc]
        private void MarkPositionsInAttackRangeTargetRpc(NetworkConnection connection)
        {
            MarkPositionsInAttackRange();
        }

        [TargetRpc]
        private void UnmarkMovementAndAttackPositionsTargetRpc(NetworkConnection connection)
        {
            UnmarkReachablePositions();
            UnmarkPositionsInAttackRange();
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

        [ServerRpc]
        private void AttackUnitAtPositionServerRpc(Vector3Int position)
        {
            int distance = GridManager.DistanceBetweenPositions(position, ActingUnit.CellPosition);
            if (distance > ActingUnit.AttackRange) return;

            BattleUnit enemyUnit = EnemyUnitsCollection.FirstOrDefault(unit => unit.CellPosition == position);
            if (enemyUnit == null) return;

            UnmarkMovementAndAttackPositionsTargetRpc(Owner);
            Vector3 enemyUnitWorldPosition = _gridManager.CellPositionToWorld(enemyUnit.CellPosition);
            AnimationType animationType = distance == 1 ? AnimationType.AttackMelee : AnimationType.AttackRanged;
            ActingUnit.AttackUnitAtPosition(enemyUnit, enemyUnitWorldPosition, animationType);
        }

        [ServerRpc]
        private void EndTurnServerRpc()
        {
            _playerMoved = false;
            ActingUnit.OnDestinationReached -= SetPlayerMoved;
            ActingUnit.OnDestinationReached -= MarkPositionsInAttackRangeTargetRpc;
            
            OnTurnEnded?.Invoke(this);
        }
    }
}