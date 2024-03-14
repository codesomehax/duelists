using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Battle
{
    public partial class BattleManager : NetworkBehaviour
    {
        private readonly IDictionary<NetworkConnection, PlayerManager> _playerManagers =
            new Dictionary<NetworkConnection, PlayerManager>(2);

        private void Awake()
        {
            PlayerManager.OnReady += SetPlayerReady;
        }

        private void SetPlayerReady(PlayerManager playerManager)
        {
            _playerManagers[playerManager.Owner] = playerManager;
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
        }

        private void OnDestroy()
        {
            PlayerManager.OnReady -= SetPlayerReady;
        }
    }
}