using System;
using Factions;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units;

namespace Battle
{
    public class PlayerManager : NetworkBehaviour
    {
        [SyncVar] [NonSerialized] public Faction Faction;
        [SyncObject] public readonly SyncDictionary<UnitType, int> UnitCounts = new();
        [SyncVar] [NonSerialized] private PlayerState _playerState = PlayerState.PlacingUnits;

        private GridManager _gridManager;

        public override void OnStartClient()
        {
            // Debug.Log($"Hello Owner {Owner} am I Owner {IsOwner} am I Host {IsHost}");
            SceneManager.OnLoadEnd += Setup;
        }

        private void Setup(SceneLoadEndEventArgs args)
        {
            SceneManager.OnLoadEnd -= Setup;
            _gridManager = FindObjectOfType<GridManager>();
            ActionTile3D.OnClick += TileSelected;
            
            _gridManager.HighlightAvailablePlacingSpots(IsHost);
        }

        private void TileSelected(ActionTile3D actionTile)
        {
            switch (_playerState)
            {
                case PlayerState.PlacingUnits:
                    TryPlaceUnit(actionTile);
                    break;
                case PlayerState.Waiting:
                    break;
                case PlayerState.Acting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TryPlaceUnit(ActionTile3D actionTile)
        {
            if (actionTile.Occupied) return;
            
        }

        private void OnDestroy()
        {
            ActionTile3D.OnClick -= TileSelected;
        }
    }

    public enum PlayerState
    {
        PlacingUnits,
        Waiting,
        Acting
    }
}