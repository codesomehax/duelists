using System.Collections.Generic;
using System.Linq;
using Factions;
using Units;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private List<BattleUnitData> unitInfos;
        [SerializeField] private List<BattleUnit> units;

        public ICollection<BattleUnitData> GetUnitsInfoByFaction(Faction faction)
        {
            return unitInfos
                .Where(unit => unit.Faction == faction)
                .AsReadOnlyCollection();
        }

        public BattleUnit GetUnitPrefabByFactionAndType(Faction faction, UnitType unitType)
        {
            return units.First(unit => unit.Faction == faction && unit.UnitType == unitType);
        }
    }
}