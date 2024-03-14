using System.Collections.Generic;
using FishNet.Component.Observing;
using Units;

namespace Battle
{
    public partial class BattleManager
    {
        private const int MaxTurnsCount = 22;

        private readonly SortedList<BattleUnit, BattleUnit> _sortedTurnList = 
            new(MaxTurnsCount, new BattleUnitTimelinePositionComparer());
        
        private void MakeUnitsObservableAndListed()
        {
            foreach (PlayerManager playerManager in _playerManagers.Values)
            {
                foreach (BattleUnit battleUnit in playerManager.BattleUnitsCollection)
                {
                    battleUnit.NetworkObserver.GetObserverCondition<OwnerOnlyCondition>().SetIsEnabled(false);
                    _sortedTurnList.Add(battleUnit, battleUnit);
                }
            }
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