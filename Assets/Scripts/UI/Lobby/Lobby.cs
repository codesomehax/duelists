using System.Collections.Generic;
using System.Linq;
using Battle;
using Factions;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using Network;
using UI.Menu;
using Units;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Lobby
{
    public partial class Lobby : NetworkBehaviour
    {
        private const string BackgroundObjectName = "Background";
        private const string BattlefieldSceneName = "Forest";

        [SerializeField] private BattleManager battleManagerPrefab;
        [SerializeField] private PlayerManager playerManagerPrefab;
        
        private LanMenu _lanMenu;
        private NetworkSetupManager _networkSetupManager;

        private void Awake()
        {
            _lanMenu = FindObjectOfType<LanMenu>(true);
            _networkSetupManager = FindObjectOfType<NetworkSetupManager>();
        }

        public override void OnStartClient()
        {
            SetupParentLocally();
            if (!IsHost)
                DestroyStartGameButton();
        }

        private void SetupParentLocally()
        {
            Transform backgroundTransform = GameObject.Find(BackgroundObjectName).transform;
            transform.SetParent(backgroundTransform, false);
        }

        public void LeaveLobby()
        {
            if (IsHost)
                _networkSetupManager.LeaveAsHost();
            else
                _networkSetupManager.LeaveAsClient();
        }

        public override void OnStopClient()
        {
            gameObject.SetActive(false);
            _lanMenu.gameObject.SetActive(true);
        }
        
        public void StartGame()
        {
            PlayerPanel[] playerPanels = FindObjectsByType<PlayerPanel>(FindObjectsSortMode.None);
            
            // if (NotEnoughPlayers(playerPanels))
            // {
            //     // TODO Popup
            //     Debug.Log("Popup message: Only a game with 2 players can be started");
            //     return;
            // }
            //
            // if (AnyTooMuchGoldSpent(playerPanels))
            // {
            //     // TODO popup
            //     Debug.Log("Popup message: There is a player that has spent too much gold");
            //     return;
            // }
            //
            // if (AnyNoUnitsSelected(playerPanels))
            // {
            //     // TODO group
            //     Debug.Log("Popup message: There is a player that has not selected a single unit");
            //     return;
            // }
            
            NetworkConnection[] connections = playerPanels.Select(playerPanel => playerPanel.Owner).ToArray();
            Army[] armies = playerPanels.Select(playerPanel => new Army()
            {
                Connection = playerPanel.Owner,
                Faction = playerPanel.Faction,
                UnitCounts = new Dictionary<UnitType, int>(playerPanel.UnitCounts)
            }).ToArray();
            SceneLoadData sld = new SceneLoadData(BattlefieldSceneName)
            {
                PreferredActiveScene = new SceneLookupData(BattlefieldSceneName),
                ReplaceScenes = ReplaceOption.All,
                Params =
                {
                    ServerParams = new object[] { armies }
                }
            };
            SceneManager.OnLoadEnd += SpawnBattleManager;
            SceneManager.LoadConnectionScenes(connections, sld);
        }

        private void SpawnBattleManager(SceneLoadEndEventArgs args)
        {
            InstanceFinder.SceneManager.OnLoadEnd -= SpawnBattleManager;
            
            BattleManager battleManager = Instantiate(battleManagerPrefab);
            InstanceFinder.ServerManager.Spawn(battleManager.NetworkObject);

            if (args.QueueData.SceneLoadData.Params.ServerParams[0] is Army[] armies)
            {
                foreach (Army army in armies)
                {
                    PlayerManager playerManager = Instantiate(playerManagerPrefab);
                    playerManager.Faction = army.Faction;
                    playerManager.UnitCounts = army.UnitCounts;
                    InstanceFinder.ServerManager.Spawn(playerManager.NetworkObject, army.Connection);
                }
            }
            else
            {
                // TODO decent message
                Debug.LogError("Armies did not get deserialized to Army[] object");
            }
        }

        private static bool NotEnoughPlayers(PlayerPanel[] playerPanels) => playerPanels.Length != 2;
        private static bool AnyTooMuchGoldSpent(PlayerPanel[] playerPanels) =>
            playerPanels.Any(playerPanel => playerPanel.Gold < 0);
        private static bool AnyNoUnitsSelected(PlayerPanel[] playerPanels) =>
            playerPanels.Any(playerPanel => playerPanel.UnitCounts.Values.Sum() == 0);
    }

    public struct Army
    {
        public NetworkConnection Connection;
        public Faction Faction;
        public Dictionary<UnitType, int> UnitCounts;
    }
}