using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Object;
using Units;
using Unity.VisualScripting;

namespace Battle
{
    public class BattleManager : NetworkBehaviour
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
            if (_playerManagers.Count == 2)
                StartGame();
        }

        private void StartGame()
        {
            foreach (NetworkConnection connection in _playerManagers.Keys)
            {
                IEnumerable<BattleUnit> battleUnits = connection.Objects
                    .Select(nob => nob != null ? nob.gameObject.GetComponent<BattleUnit>() : null)
                    .NotNull();
                foreach (BattleUnit battleUnit in battleUnits)
                    battleUnit.NetworkObserver.GetObserverCondition<OwnerOnlyCondition>().SetIsEnabled(false);
            }
        }

        private void OnDestroy()
        {
            PlayerManager.OnReady -= SetPlayerReady;
        }
    }
}