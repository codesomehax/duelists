using System;
using System.Collections.Generic;
using System.Linq;
using Battle.UI;
using Factions;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units;
using UnityEngine;

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
        private ActionTile3D _currentActionTile;

        public override void OnStartNetwork()
        {
            SceneManager.OnLoadEnd += Setup;
        }

        private void Setup(SceneLoadEndEventArgs args)
        {
            SceneManager.OnLoadEnd -= Setup;
            _gridManager = FindObjectOfType<GridManager>();
            _battleUIManager = FindObjectOfType<BattleUIManager>();
            _unitsManager = FindObjectOfType<UnitsManager>();

            if (args.QueueData.AsServer) return;
            
            ActionTile3D.OnClick += TileSelected;
            _battleUIManager.OnUnitPlaced += PlaceUnit;
            
            _gridManager.HighlightAvailablePlacingSpots(IsHost);
        }

        private void PlaceUnit(UnitType unitType, int unitCount)
        {
            PlaceUnitServerRpc(_currentActionTile.GridPosition, unitType, unitCount);
        }

        [ServerRpc]
        private void PlaceUnitServerRpc(Vector3Int actionTileCellPosition, UnitType unitType, int unitCount)
        {
            if (unitCount > AvailableUnits[unitType])
            {
                Debug.LogWarning("Unit cannot be placed: provided unitCount is higher than AvailableCount");
                return;
            }

            if (unitCount <= 0)
            {
                Debug.LogWarning("Unit cannot be placed: provided unitCount is too low");
                return;
            }

            AvailableUnits[unitType] -= unitCount;
            
            BattleUnit unitPrefab = _unitsManager.GetUnitPrefabByFactionAndType(Faction, unitType);
            BattleUnit unit = Instantiate(unitPrefab);
            unit.Count = unitCount;
            _gridManager.PlaceUnit(unit, actionTileCellPosition, Owner.IsHost);
            Spawn(unit.NetworkObject, Owner);
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
            if (actionTile.Occupied) return; // TODO add functionality to remove
            ICollection<BattleUnitInfo> units = _unitsManager.GetUnitsInfoByFaction(Faction);
            Debug.Log(units);
            IDictionary<UnitType, PlaceUnitData> placeUnitData = units.ToDictionary(
                unit => unit.UnitType,
                unit => new PlaceUnitData()
                {
                    Name = unit.Name,
                    Count = AvailableUnits[unit.UnitType]
                });
            _currentActionTile = actionTile;
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