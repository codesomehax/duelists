using System;
using System.Collections.Generic;
using System.Linq;
using Battle.UI;
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
        [SyncObject] public readonly SyncDictionary<UnitType, int> AvailableUnits = new();

        #region RuntimeDependencies
        private GridManager _gridManager;
        private BattleUIManager _battleUIManager;
        private UnitsManager _unitsManager;
        #endregion
        
        private PlayerState _playerState = PlayerState.PlacingUnits;

        public override void OnStartClient()
        {
            // Debug.Log($"Hello Owner {Owner} am I Owner {IsOwner} am I Host {IsHost}");
            SceneManager.OnLoadEnd += Setup;
        }

        private void Setup(SceneLoadEndEventArgs args)
        {
            SceneManager.OnLoadEnd -= Setup;
            _gridManager = FindObjectOfType<GridManager>();
            _battleUIManager = FindObjectOfType<BattleUIManager>();
            _unitsManager = FindObjectOfType<UnitsManager>();
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
            ICollection<BattleUnitData> units = _unitsManager.GetUnitsByFaction(Faction);
            IDictionary<UnitType, PlaceUnitData> placeUnitData = units.ToDictionary(
                unit => unit.UnitType,
                unit => new PlaceUnitData()
                {
                    Name = unit.Name,
                    Count = AvailableUnits[unit.UnitType]
                });
            _battleUIManager.ShowPlaceUnitWindow(placeUnitData);
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