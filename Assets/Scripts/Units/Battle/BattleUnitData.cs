using Factions;
using UnityEngine;

namespace Units.Battle
{
    [CreateAssetMenu(fileName = "BattleUnitData", menuName = "Duelists/Battle Unit Data", order = 1)]
    public class BattleUnitData : ScriptableObject
    {
        public const int MaxStrength = 100;
        public const uint MaxAgility = 99;
        public const int MaxIntelligence = 100;
        public const int MaxPhysicalDefense = 70;
        public const int MaxMagicDefense = 70;
        
        [Header("General")]
        [SerializeField] private new string name;
        [SerializeField] private Faction faction;
        [SerializeField] private UnitType unitType;

        [Header("Stats")]
        [SerializeField] private int singleUnitHealth;
        [SerializeField] private int baseDamage;
        [SerializeField] private int strength;
        [SerializeField] private uint agility;
        [SerializeField] private int intelligence;
        [SerializeField] private int physicalDefense;
        [SerializeField] private int magicDefense;
        [SerializeField] private int speed;
        [SerializeField] private float movementSpeed;
        [SerializeField] private int attackRange;

        [Header("Attack types")]
        [SerializeField] private AttackType meleeAttackType;
        [SerializeField] private AttackType rangedAttackType;

        public string Name => name;
        public Faction Faction => faction;
        public UnitType UnitType => unitType;

        public int SingleUnitHealth => singleUnitHealth;
        public int BaseDamage => baseDamage;
        public int Strength => strength;
        public uint Agility => agility;
        public int Intelligence => intelligence;
        public int PhysicalDefense => physicalDefense;
        public int MagicDefense => magicDefense;
        public int Speed => speed;
        public float MovementSpeed => movementSpeed;
        public int AttackRange => attackRange;
        
        public AttackType MeleeAttackType => meleeAttackType;
        public AttackType RangedAttackType => rangedAttackType;
    }
}