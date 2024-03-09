using System.Collections.Generic;
using Factions;
using FishNet.Connection;
using Units;

namespace Battle
{
    public struct Army
    {
        public NetworkConnection Connection;
        public Faction Faction;
        public Dictionary<UnitType, int> UnitCounts;
    }
}