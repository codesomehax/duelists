using System.Collections.Generic;
using System.Linq;
using Battle.Player;
using FishNet.Component.Observing;
using Units;
using Units.Battle;
using UnityEngine;

namespace Battle
{
    public partial class BattleManager
    {
        private const int MaxTurnsCount = 22;

        private readonly SortedList<BattleUnit, BattleUnit> _sortedTurnList = 
            new(MaxTurnsCount, new BattleUnitTimelinePositionComparer());
        
        private void MakeUnitsObservableAndListed()
        {
            foreach (PlayerManager playerManager in _playerManagers)
            {
                foreach (BattleUnit battleUnit in playerManager.BattleUnitsCollection)
                {
                    battleUnit.NetworkObserver.GetObserverCondition<OwnerOnlyCondition>().SetIsEnabled(false);
                    _sortedTurnList.Add(battleUnit, battleUnit);
                }
            }
        }

        private void NextTurn()
        {
            BattleUnit battleUnit = _sortedTurnList.Values[0];
            PlayerManager playerManager = _playerManagers.First(pm => pm.Owner == battleUnit.Owner);
            playerManager.ActingUnit = battleUnit;
            playerManager.PlayerState = PlayerState.Acting;
        }
    }

    public class BattleUnitTimelinePositionComparer : Comparer<BattleUnit>
    {
        public override int Compare(BattleUnit x, BattleUnit y)
        {
            if (x == null)
            {
                if (y == null) return 0;
                return -1;
            }
            if (y == null) return 1;

            if (x.TimelinePosition == y.TimelinePosition) return 1;
            
            return x.TimelinePosition.CompareTo(y.TimelinePosition);
        }
    }
}