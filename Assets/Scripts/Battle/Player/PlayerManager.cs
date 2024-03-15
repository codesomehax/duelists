﻿using System;
using System.Collections.Generic;
using System.Linq;
using Factions;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units;
using Units.Battle;
using Unity.VisualScripting;

namespace Battle.Player
{
    public partial class PlayerManager : NetworkBehaviour
    {
        private const int MaxBattleUnitsCount = 10;

        [SyncVar] [NonSerialized] public Faction Faction;
        [SyncObject] public readonly SyncDictionary<UnitType, int> AvailableUnits = new();
        [SyncVar(OnChange = nameof(StartTurn))] [NonSerialized] public PlayerState PlayerState = PlayerState.PlacingUnits;

        public ICollection<BattleUnit> BattleUnitsCollection => Owner.Objects
            .Select(nob => nob != null ? nob.gameObject.GetComponent<BattleUnit>() : null)
            .NotNull()
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
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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