using System;
using System.Collections.Generic;
using Factions;
using FishNet.Component.Animating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units.Battle.Specialists.SpellCaster;
using Units.Hero;
using UnityEngine;

namespace Units.Battle
{
    public partial class BattleUnit : NetworkBehaviour
    {
        public static event Action<Vector3Int> OnUnitPlaced;
        public static event Action<Vector3Int> OnUnitRemoval;

        #region Synchronized
        [SyncVar(OnChange = nameof(SyncUnitCount))] [NonSerialized] public int Count;
        [SyncVar] [NonSerialized] public Vector3Int CellPosition;
        [SyncVar] [NonSerialized] public ulong TimelinePosition;
        [SyncVar] [NonSerialized] public HeroType HeroType;
        [SyncVar] [NonSerialized] public AbilityType AbilityType;
        #endregion

        #region General
        public string Name => battleUnitData.Name;
        public Faction Faction => battleUnitData.Faction;
        public UnitType UnitType => battleUnitData.UnitType;
        public BattleUnitData BattleUnitData => battleUnitData;
        public bool IsSpellCaster => _spellCaster != null;
        public bool HasCastedSpell => _spellCaster.IsSpellCasted;
        #endregion

        #region Stats

        private static readonly IDictionary<HeroType, int> StrengthPerHeroType = new Dictionary<HeroType, int>
        {
            { HeroType.Warrior, 30 },
            { HeroType.Ranger, 15 },
            { HeroType.Mage, 10 }
        };
        private static readonly IDictionary<HeroType, uint> AgilityPerHeroType = new Dictionary<HeroType, uint>
        {
            { HeroType.Warrior, 20 },
            { HeroType.Ranger, 30 },
            { HeroType.Mage, 20 }
        };
        private static readonly IDictionary<HeroType, int> IntelligencePerHeroType = new Dictionary<HeroType, int>
        {
            { HeroType.Warrior, 10 },
            { HeroType.Ranger, 15 },
            { HeroType.Mage, 30 }
        };

        private int AbilityStrengthIncrease => AbilityType == AbilityType.StrengthIncrease ? 10 : 0;
        private uint AbilityAgilityIncrease => (uint)(AbilityType == AbilityType.AgilityIncrease ? 10 : 0);
        private int AbilityIntelligenceIncrease => AbilityType == AbilityType.IntelligenceIncrease ? 10 : 0;
        
        public int SingleUnitHealth => battleUnitData.SingleUnitHealth;
        public int BaseDamage => battleUnitData.BaseDamage;
        public int Strength => battleUnitData.Strength + StrengthPerHeroType[HeroType] + AbilityStrengthIncrease;
        public uint Agility => battleUnitData.Agility + AgilityPerHeroType[HeroType] + AbilityAgilityIncrease;
        public int Intelligence => battleUnitData.Intelligence + IntelligencePerHeroType[HeroType] 
                                                               + AbilityIntelligenceIncrease;
        public int PhysicalDefense => battleUnitData.PhysicalDefense;
        public int MagicDefense => battleUnitData.MagicDefense;
        public int Speed => battleUnitData.Speed;
        public int AttackRange => battleUnitData.AttackRange;

        public AttackType MeleeAttackType => battleUnitData.MeleeAttackType;
        public AttackType RangedAttackType => battleUnitData.RangedAttackType;
        
        // Computed
        public int Health { get; set; }
        private int PhysicalDamage => Mathf.CeilToInt(BaseDamage * Count * ((float)(100 + Strength) / 100));
        private int MagicDamage => Mathf.CeilToInt(BaseDamage * Count * ((float)(100 + Intelligence) / 100));
        
        #endregion

        #region Internal
        private uint Period => BattleUnitData.MaxAgility + 1 - Agility;
        private SpellCaster _spellCaster;
        #endregion

        #region Serialized
        [Header("Unit")]
        [SerializeField] private BattleUnitData battleUnitData;
        #endregion
        
        private void Awake()
        {
            _cameraTransform = FindObjectOfType<Camera>().transform;
            _networkAnimator = GetComponent<NetworkAnimator>();
            _spellCaster = GetComponent<SpellCaster>();
        }

        public override void OnStartServer()
        {
            DisableClientCanvasOnHost();
            RotateTowardsEnemySide();
            IncrementTimelinePosition();
        }

        public override void OnStartClient()
        {
            SetupIcon();
            ShowCanvas();
            if (IsOwner)
                OnUnitPlaced?.Invoke(CellPosition);
        }

        private void Update()
        {
            RotateCanvasToCamera();
        }

        public void IncrementTimelinePosition() => TimelinePosition += Period;

        public override void OnStopClient()
        {
            if (Count > 0)
                OnUnitRemoval?.Invoke(CellPosition);
        }
    }
}