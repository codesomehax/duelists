using System;
using Factions;
using FishNet.Component.Observing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Units
{
    public partial class BattleUnit : NetworkBehaviour
    {
        public static event Action<Vector3Int> OnUnitPlaced;
        public static event Action<Vector3Int> OnUnitRemoval;

        [SyncVar(OnChange = nameof(SyncUnitCount))] [NonSerialized] public int Count;
        [SyncVar] [NonSerialized] public Vector3Int CellPosition;

        public string Name => battleUnitInfo.Name;
        public Faction Faction => battleUnitInfo.Faction;
        public UnitType UnitType => battleUnitInfo.UnitType;
        
        [Header("Unit")]
        [SerializeField] private BattleUnitInfo battleUnitInfo;

        private void Awake()
        {
            _cameraTransform = FindObjectOfType<Camera>().transform;
        }

        public override void OnStartServer()
        {
            DisableClientCanvasOnHost();
        }

        public override void OnStartClient()
        {
            Debug.Log($"Running as {LocalConnection} with Owner of unit {Owner}");
            ShowCanvas();
            if (IsOwner)
                OnUnitPlaced?.Invoke(CellPosition);
        }

        private void Update()
        {
            RotateCanvasToCamera();
        }

        public override void OnStopClient()
        {
            OnUnitRemoval?.Invoke(CellPosition);
        }
    }
}