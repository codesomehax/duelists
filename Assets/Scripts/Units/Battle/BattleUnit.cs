using System;
using Factions;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Units.Battle
{
    public partial class BattleUnit : NetworkBehaviour
    {
        public static event Action<Vector3Int> OnUnitPlaced;
        public static event Action<Vector3Int> OnUnitRemoval;

        private const uint MaxAgility = 99;

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
        // TODO temporary values, they need to be affected by hero etc
        // TODO these values need to be synchronized!!!
        public uint Strength => battleUnitData.Strength;
        public uint Agility => battleUnitData.Agility;
        public uint Intelligence => battleUnitData.Intelligence;
        public uint PhysicalDefense => battleUnitData.PhysicalDefense;
        public uint MagicDefense => battleUnitData.MagicDefense;
        public int Speed => battleUnitData.Speed;
        #endregion

        #region Internal
        private uint Period => MaxAgility + 1 - Agility;
        #endregion

        #region Serialized
        [Header("Unit")]
        [SerializeField] private BattleUnitData battleUnitData;
        #endregion
        
        private void Awake()
        {
            _cameraTransform = FindObjectOfType<Camera>().transform;
        }

        public override void OnStartServer()
        {
            DisableClientCanvasOnHost();
            RotateTowardsEnemy();
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