using System;
using System.Collections.Generic;
using System.Linq;
using Battle.UI;
using FishNet.Object;
using Units;
using UnityEngine;

namespace Battle
{
    public partial class PlayerManager
    {
        public static event Action<PlayerManager> OnReady;
        
        private void StartPlacingUnit(ActionTile3D actionTile)
        {
            // Checks if it is occupied
            // TODO add remove functionality if matches
            if (BattleUnitsCollection.Any(battleUnit => battleUnit.CellPosition == actionTile.CellPosition))
            {
                _currentActionTile = actionTile;
                AskRemoveUnitWindow();
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
            
            ICollection<BattleUnitData> units = _unitsManager.GetUnitsInfoByFaction(Faction);
            IDictionary<UnitType, PlaceUnitData> placeUnitData = units.ToDictionary(
                unit => unit.UnitType,
                unit => new PlaceUnitData()
                {
                    Name = unit.Name,
                    Count = AvailableUnits[unit.UnitType]
                });
            _currentActionTile = actionTile;
            ShowPlaceUnitWindow(placeUnitData);
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
            _gridManager.PlaceUnit(unit, cellPosition);
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

        [ServerRpc]
        private void SetPlayerReadyServerRpc()
        {
            OnReady?.Invoke(this);
        }
    }
}