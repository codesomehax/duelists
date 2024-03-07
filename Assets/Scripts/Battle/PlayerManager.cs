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

        private GridManager _gridManager;

        public override void OnStartClient()
        {
            // Debug.Log($"Hello Owner {Owner} am I Owner {IsOwner} am I Host {IsHost}");
            SceneManager.OnLoadEnd += PlaceUnits;
        }

        private void PlaceUnits(SceneLoadEndEventArgs args)
        {
            SceneManager.OnLoadEnd -= PlaceUnits;
            _gridManager = FindObjectOfType<GridManager>();
            
            _gridManager.HighlightAvailablePlacingSpots(IsHost);
        }
    }
}