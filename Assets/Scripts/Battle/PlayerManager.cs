﻿using System;
using System.Collections.Generic;
using System.Linq;
using Battle.UI;
using Factions;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle
{
    public partial class PlayerManager : NetworkBehaviour
    {
        private const int MaxBattleUnitsCount = 10;

        [SyncVar] [NonSerialized] public Faction Faction;
        [SyncObject] public readonly SyncDictionary<UnitType, int> AvailableUnits = new();

        private ICollection<BattleUnit> BattleUnitsCollection => Owner.Objects
            .Select(nob => nob != null ? nob.gameObject.GetComponent<BattleUnit>() : null)
            .NotNull()
            .AsReadOnlyCollection();

        #region RuntimeDependencies
        private GridManager _gridManager;
        private UnitsManager _unitsManager;
        #endregion
        
        private PlayerState _playerState = PlayerState.PlacingUnits;
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
            switch (_playerState)
            {
                case PlayerState.PlacingUnits:
                    StartPlacingUnit(actionTile);
                    break;
                case PlayerState.Waiting:
                    break;
                case PlayerState.Acting:
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