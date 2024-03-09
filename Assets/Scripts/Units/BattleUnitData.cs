using Factions;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "BattleUnitData", menuName = "Duelists/BattleUnitData", order = 1)]
    public class BattleUnitData : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private Faction faction;
        [SerializeField] private UnitType unitType;

        public string Name => name;
        public Faction Faction => faction;
        public UnitType UnitType => unitType;
    }
}