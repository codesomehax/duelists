using UnityEngine;
using UnityEngine.Tilemaps;

namespace Battle
{
    [RequireComponent(typeof(Tilemap))]
    public class ActionTilemapManager : MonoBehaviour
    {
        // Dimensions of the tilemap:
        // Left bottom: (5, -5)
        // Left top: (-4, -5)
        // Right bottom: (5, 6)
        // Right top: (-4, 6)
        private static readonly Vector3Int LeftBottom = new(5, -5);
        private static readonly Vector3Int LeftTop = new(-4, -5);
        private static readonly Vector3Int RightBottom = new(5, 6);
        private static readonly Vector3Int RightTop = new(-4, 6);

        private const int UpDirection = -1;
        private const int RightDirection = 1;
        
        [SerializeField] private TileBase greenTile;

        private Tilemap _tilemap;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
        }

        // private void SpawnBattleManager(ClientPresenceChangeEventArgs args)
        // {
        //     if (!args.QueueData.AsServer) return;
        //
        //     if (args.QueueData.SceneLoadData.Params.ServerParams[0] is not ArmyData[] armyDataCollection) return;
        //     
        //     int hostId = InstanceFinder.ClientManager.Connection.ClientId;
        //     ArmyData hostArmyData = armyDataCollection.First(WithClientId(hostId));
        //     ArmyData clientArmyData = armyDataCollection.First(WithDifferentClientId(hostId));
        //     
        //     BattleManager battleManager = Instantiate(battleManagerPrefab);
        //     battleManager.HostArmyData = hostArmyData;
        //     battleManager.ClientArmyData = clientArmyData;
        //     InstanceFinder.ServerManager.Spawn(battleManager.NetworkObject);
        // }

        // private static Func<ArmyData, bool> WithClientId(int clientId) 
        //     => armyData => armyData.Connection.ClientId == clientId;
        // private static Func<ArmyData, bool> WithDifferentClientId(int clientId) 
        //     => armyData => armyData.Connection.ClientId != clientId;

        // public void StartPlacingLocalArmy(ArmyData armyData)
        // {
        //     _tilemap.BoxFill(LeftBottom, greenTile, -10, -10, 10, 10);
        // }
    }
}