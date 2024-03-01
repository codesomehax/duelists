using System.Collections.Generic;
using System.Linq;
using Factions;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using Network;
using UI.Menu;
using Units;
using UnityEngine;

namespace UI.Lobby
{
    public partial class Lobby : NetworkBehaviour
    {
        private const string BackgroundObjectName = "Background";
        private const string BattlefieldSceneName = "Forest";

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
                DisableStartGameButton();
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
            
            if (NotEnoughPlayers(playerPanels))
            {
                // TODO Popup
                Debug.Log("Popup message: Only a game with 2 players can be started");
                return;
            }

            if (AnyTooMuchGoldSpent(playerPanels))
            {
                // TODO popup
                Debug.Log("Popup message: There is a player that has spent too much gold");
                return;
            }

            if (AnyNoUnitsSelected(playerPanels))
            {
                // TODO group
                Debug.Log("Popup message: There is a player that has not selected a single unit");
                return;
            }
            
            IEnumerable<ArmyData> armyDataArray = playerPanels.Select(playerPanel => new ArmyData()
            {
                Connection = playerPanel.Owner,
                Faction = playerPanel.Faction,
                UnitCounts = playerPanel.UnitCounts
            });
            
            NetworkConnection[] connections = playerPanels.Select(playerPanel => playerPanel.Owner).ToArray();
            SceneLoadData sld = new SceneLoadData(BattlefieldSceneName)
            {
                ReplaceScenes = ReplaceOption.All,
                Params =
                {
                    ServerParams = new object[] { armyDataArray }
                }
            };
            SceneManager.LoadConnectionScenes(connections.ToArray(), sld);
        }
        
        private static bool NotEnoughPlayers(PlayerPanel[] playerPanels) => playerPanels.Length != 2;
        private static bool AnyTooMuchGoldSpent(PlayerPanel[] playerPanels) =>
            playerPanels.Any(playerPanel => playerPanel.Gold < 0);
        private static bool AnyNoUnitsSelected(PlayerPanel[] playerPanels) =>
            playerPanels.Any(playerPanel => playerPanel.UnitCounts.Values.Sum() == 0);
    }

    public struct ArmyData
    {
        public NetworkConnection Connection;
        public Faction Faction;
        public IDictionary<UnitType, int> UnitCounts;
    }
}