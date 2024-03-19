using System;
using System.Collections.Generic;
using System.Linq;
using Factions;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units;
using Units.Battle;
using Units.Hero;
using Unity.VisualScripting;

namespace Battle.Player
{
    public partial class PlayerManager : NetworkBehaviour
    {
        private const string StartMenuSceneName = "Start Menu";
        private const int MaxBattleUnitsCount = 10;

        [SyncVar] [NonSerialized] public Faction Faction;
        [SyncVar] [NonSerialized] public HeroType HeroType;
        [SyncVar] [NonSerialized] public AbilityType AbilityType;
        [SyncObject] public readonly SyncDictionary<UnitType, int> AvailableUnits = new();
        [SyncVar(OnChange = nameof(StartTurn))] [NonSerialized] public PlayerState PlayerState = PlayerState.PlacingUnits;
        [SyncVar] [NonSerialized] public BattleUnit ActingUnit;
        [SyncVar] private bool _playerMoved;

        public ICollection<BattleUnit> BattleUnitsCollection => Owner.Objects
            .Select(nob => nob != null ? nob.gameObject.GetComponent<BattleUnit>() : null)
            .NotNull()
            .AsReadOnlyCollection();

        public ICollection<BattleUnit> EnemyUnitsCollection => FindObjectsOfType<BattleUnit>()
            .Where(unit => unit.Owner != Owner)
            .AsReadOnlyCollection();

        #region RuntimeDependencies
        private GridManager _gridManager;
        private UnitsManager _unitsManager;
        #endregion
        
        private ActionTile3D _currentActionTile;

        public override void OnStartNetwork()
        {
            SceneManager.OnLoadEnd += Setup;
        }

        private void Setup(SceneLoadEndEventArgs args)
        {
            SceneManager.OnLoadEnd -= Setup;
            _gridManager = FindObjectOfType<GridManager>();
            _unitsManager = FindObjectOfType<UnitsManager>();

            if (args.QueueData.AsServer || !IsOwner) return;

            ActionTile3D.OnClick += TileSelected;
            SetupUI();

            _gridManager.HighlightAvailablePlacingSpots(IsHost);
        }

        private void TileSelected(ActionTile3D actionTile)
        {
            switch (PlayerState)
            {
                case PlayerState.PlacingUnits:
                    StartPlacingUnit(actionTile);
                    break;
                case PlayerState.Waiting:
                    break;
                case PlayerState.Acting:
                    if (actionTile.ActionTileState == ActionTileState.Available)
                        MoveActingUnitToPositionServerRpc(actionTile.CellPosition);
                    if (actionTile.ActionTileState == ActionTileState.Attack)
                        AttackUnitAtPositionServerRpc(actionTile.CellPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnStopClient()
        {
            if (IsOwner)
                UnityEngine.SceneManagement.SceneManager.LoadScene(StartMenuSceneName);
            else if (IsServer)
            {
                ClientManager.StopConnection();
                ServerManager.StopConnection(true);
            }
        }

        private void OnDestroy()
        {
            ActionTile3D.OnClick -= TileSelected;
        }
    }

    public enum PlayerState
    {
        PlacingUnits,
        Waiting,
        Acting
    }
}