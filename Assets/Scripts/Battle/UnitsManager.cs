using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Factions;
using Units;
using Units.Battle;
using Units.Hero;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private List<BattleUnit> units;

        [SerializedDictionary("Hero type", "Heaven Hero")] 
        [SerializeField]
        private SerializedDictionary<HeroType, BattleUnit> heavenHeroes;
        
        [SerializedDictionary("Hero type", "Hell Hero")] 
        [SerializeField]
        private SerializedDictionary<HeroType, BattleUnit> hellHeroes;

        public ICollection<BattleUnitData> GetUnitsInfoByFaction(Faction faction)
        {
            return units
                .Where(unit => unit.Faction == faction)
                .Select(unit => unit.BattleUnitData)
                .ToList();
        }

        public BattleUnit GetUnitPrefabByFactionAndType(Faction faction, UnitType unitType)
        {
            return units.First(unit => unit.Faction == faction && unit.UnitType == unitType);
        }

        public BattleUnitData GetHeroInfoByFactionAndType(Faction faction, HeroType heroType)
        {
            return faction == Faction.Heaven 
                ? heavenHeroes[heroType].BattleUnitData
                : hellHeroes[heroType].BattleUnitData;
        }
        
        public BattleUnit GetHeroByFactionAndType(Faction faction, HeroType heroType)
        {
            return faction == Faction.Heaven 
                ? heavenHeroes[heroType]
                : hellHeroes[heroType];
        }
    }
}