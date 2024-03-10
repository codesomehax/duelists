using System;
using System.Collections.Generic;
using System.Linq;
using Battle.UI;
using Factions;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle
{
    public class PlayerManager : NetworkBehaviour
    {
        private const int MaxBattleUnitsCount = 10;
        
        [SyncVar] [NonSerialized] public Faction Faction;
        [SyncObject] public readonly SyncDictionary<UnitType, int> AvailableUnits = new();

        private ICollection<BattleUnit> BattleUnitsCollection => Owner.Objects
            .Select(nob => nob != null ? nob.gameObject.GetComponent<BattleUnit>() : null)
            .NotNull()
            .AsReadOnlyCollection();

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

            if (args.QueueData.AsServer || !IsOwner) return;

            ActionTile3D.OnClick += TileSelected;
            _battleUIManager.OnUnitPlaced += PlaceUnit;
            _battleUIManager.OnUnitRemovalConfirmed += RemoveUnit;
            
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
            if (BattleUnitsCollection.Any(battleUnit => battleUnit.CellPosition == actionTile.CellPosition))
            {
                _currentActionTile = actionTile;
                _battleUIManager.AskRemoveUnit();
                return;
            }

            if (BattleUnitsCollection.Count == MaxBattleUnitsCount)
            {
                Debug.Log($"No more than {MaxBattleUnitsCount} units can be placed");
                return;
            }

            if (actionTile.ActionTileState != ActionTileState.Available)
            {
                Debug.Log("This tile is already occupied");
                return;
            }
            
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

            if (BattleUnitsCollection.Count == MaxBattleUnitsCount)
            {
                Debug.LogWarning($"Unit cannot be placed: already {MaxBattleUnitsCount} units present");
                return;
            }

            if (BattleUnitsCollection.Any(battleUnit => battleUnit.CellPosition == cellPosition))
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
            AvailableUnits[unitType] -= unitCount;
        }

        private void RemoveUnit()
        {
            RemoveUnitServerRpc(_currentActionTile.CellPosition);
        }

        [ServerRpc]
        private void RemoveUnitServerRpc(Vector3Int cellPosition)
        {
            BattleUnit battleUnit = BattleUnitsCollection.First(unit => unit.CellPosition == cellPosition);
            AvailableUnits[battleUnit.UnitType] += battleUnit.Count;
            Despawn(battleUnit.NetworkObject);
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