﻿using System.Collections.Generic;
using System.Linq;
using Battle.Player;
using FishNet.Object;
using Units.Battle;
using UnityEngine;

namespace Battle
{
    public partial class BattleManager : NetworkBehaviour
    {
        private readonly ISet<PlayerManager> _playerManagers = new HashSet<PlayerManager>(2);

        private void Awake()
        {
            PlayerManager.OnReady += SetPlayerReady;
            BattleUnit.OnUnitDeath += DespawnUnit;
        }

        private void DespawnUnit(BattleUnit battleUnit)
        {
            _sortedTurnList.Remove(battleUnit);
            Despawn(battleUnit.NetworkObject);
        }

        private void SetPlayerReady(PlayerManager playerManager)
        {
            _playerManagers.Add(playerManager);
            playerManager.PlayerState = PlayerState.Waiting;
            if (_playerManagers.Count == 2)
                StartGame();
        }

        private void StartGame()
        {
            MakeUnitsObservableAndListed();
            SetupIconsTransformObserversRpc();
            Vector3Int[] sortedCellPositions = _sortedTurnList.Values.Select(battleUnit => battleUnit.CellPosition).ToArray();
            ArrangeUnitIconsObserversRpc(sortedCellPositions);
            NextTurn();
        }

        private void OnDestroy()
        {
            PlayerManager.OnReady -= SetPlayerReady;
            BattleUnit.OnUnitDeath -= DespawnUnit;
        }
    }
}