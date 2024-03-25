using System.Collections.Generic;
using System.Linq;
using Battle.Player;
using FishNet.Connection;
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

        private void DespawnUnit(BattleUnit battleUnit)
        {
            _sortedTurnList.RemoveAt(_sortedTurnList.IndexOfValue(battleUnit));
            
            NetworkConnection owner = battleUnit.Owner;
            
            Despawn(battleUnit.NetworkObject);
            
            PlayerManager playerManager = _playerManagers.First(pm => pm.Owner == owner);
            ICollection<BattleUnit> battleUnits = playerManager.BattleUnitsCollection;
            if (battleUnits.Count == 0 || battleUnits.Count == 1 && battleUnits.Contains(battleUnit))
            {
                LoseGameTargetRpc(owner);
                NetworkConnection winner = ServerManager.Clients.Values.First(conn => conn != owner);
                WinGameTargetRpc(winner);
            }
        }

        [TargetRpc]
        private void WinGameTargetRpc(NetworkConnection connection)
        {
            inGameMenu.ShowWinnerMenu();
        }

        [TargetRpc]
        private void LoseGameTargetRpc(NetworkConnection connection)
        {
            inGameMenu.ShowLoserMenu();
        }

        private void OnDestroy()
        {
            PlayerManager.OnReady -= SetPlayerReady;
            BattleUnit.OnUnitDeath -= DespawnUnit;
        }
    }
}