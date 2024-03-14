using Battle.UI.Elements;
using Factions;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "BattleUnitData", menuName = "Duelists/Battle Unit Data", order = 1)]
    public class BattleUnitData : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private new string name;
        [SerializeField] private Faction faction;
        [SerializeField] private UnitType unitType;

        [Header("Stats")]
        [SerializeField] private uint strength;
        [SerializeField] private uint agility;
        [SerializeField] private uint intelligence;
        [SerializeField] private uint physicalDefense;
        [SerializeField] private uint magicDefense;

        public string Name => name;
        public Faction Faction => faction;
        public UnitType UnitType => unitType;
        public uint Strength => strength;
        public uint Agility => agility;
        public uint Intelligence => intelligence;
        public uint PhysicalDefense => physicalDefense;
        public uint MagicDefense => magicDefense;
    }
}