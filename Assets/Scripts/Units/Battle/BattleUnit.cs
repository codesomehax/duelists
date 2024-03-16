using System;
using Factions;
using FishNet.Component.Animating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
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
        [field: SyncVar] [field: NonSerialized] public ulong TimelinePosition { get; private set; }
        #endregion

        #region General
        public string Name => battleUnitData.Name;
        public Faction Faction => battleUnitData.Faction;
        public UnitType UnitType => battleUnitData.UnitType;
        #endregion

        #region Stats
        
        public int SingleUnitHealth => battleUnitData.SingleUnitHealth;
        public int BaseDamage => battleUnitData.BaseDamage;
        public int Strength => battleUnitData.Strength;
        public uint Agility => battleUnitData.Agility;
        public int Intelligence => battleUnitData.Intelligence;
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
        #endregion

        #region Serialized
        [Header("Unit")]
        [SerializeField] private BattleUnitData battleUnitData;
        #endregion
        
        private void Awake()
        {
            _cameraTransform = FindObjectOfType<Camera>().transform;
            _networkAnimator = GetComponent<NetworkAnimator>();
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
            OnUnitRemoval?.Invoke(CellPosition);
        }
    }
}