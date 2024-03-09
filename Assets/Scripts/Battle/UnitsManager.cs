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
        [SerializeField] private List<BattleUnitData> units;

        public IReadOnlyCollection<BattleUnitData> Units => units.AsReadOnly();

        public ICollection<BattleUnitData> GetUnitsByFaction(Faction faction)
        {
            return units
                .Where(unit => unit.Faction == faction)
                .AsReadOnlyCollection();
        }
    }
}