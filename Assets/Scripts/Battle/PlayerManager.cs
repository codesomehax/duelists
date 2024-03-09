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
        private const int MaxBattleUnitsCount = 10;
        
        [SyncVar] [NonSerialized] public Faction Faction;
        [SyncObject] public readonly SyncDictionary<UnitType, int> AvailableUnits = new();
        [SyncObject(ReadPermissions = ReadPermission.OwnerOnly)] 
        private readonly SyncHashSet<BattleUnit> _battleUnits = new();

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
            
            if (IsOwner)
                _battleUnits.OnChange += MarkTileAsOccupied;

            ActionTile3D.OnClick += TileSelected;
            _battleUIManager.OnUnitPlaced += PlaceUnit;
            
            _gridManager.HighlightAvailablePlacingSpots(IsHost);
        }

        private void TileSelected(ActionTile3D actionTile)
        {
            switch (_playerState)
            {
                case PlayerState.PlacingUnits:
                    StartPlacingUnit(actionTile);
                    break;
                case PlayerState.Waiting:
                    break;
                case PlayerState.Acting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartPlacingUnit(ActionTile3D actionTile)
        {
            // Checks if it is occupied
            // TODO add remove functionality if matches
            if (actionTile.ActionTileState != ActionTileState.Available) return;
            if (_battleUnits.Any(battleUnit => battleUnit.CellPosition == actionTile.CellPosition)) return;
            if (_battleUnits.Count == MaxBattleUnitsCount) return;
            
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

        private void PlaceUnit(UnitType unitType, int unitCount)
        {
            PlaceUnitServerRpc(_currentActionTile.CellPosition, unitType, unitCount);
        }

        [ServerRpc]
        private void PlaceUnitServerRpc(Vector3Int cellPosition, UnitType unitType, int unitCount)
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

            if (_battleUnits.Count == MaxBattleUnitsCount)
            {
                Debug.LogWarning($"Unit cannot be placed: already {MaxBattleUnitsCount} units present");
                return;
            }

            if (_battleUnits.Any(battleUnit => battleUnit.CellPosition == cellPosition))
            {
                Debug.LogWarning("Unit cannot be placed: the tile is already occupied");
                return;
            }

            
            BattleUnit unitPrefab = _unitsManager.GetUnitPrefabByFactionAndType(Faction, unitType);
            BattleUnit unit = Instantiate(unitPrefab);
            unit.Count = unitCount;
            unit.CellPosition = cellPosition;
            _gridManager.PlaceUnit(unit, cellPosition, Owner.IsHost);
            Spawn(unit.NetworkObject, Owner);
            
            _battleUnits.Add(unit);
            AvailableUnits[unitType] -= unitCount;
        }

        private void MarkTileAsOccupied(SyncHashSetOperation operation, BattleUnit battleUnit, bool asServer)
        {
            if (operation == SyncHashSetOperation.Add && !asServer && IsOwner)
            {
                _gridManager.MarkTileAsOccupied(battleUnit.CellPosition);
            }
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