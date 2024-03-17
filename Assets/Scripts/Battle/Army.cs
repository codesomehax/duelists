using System.Collections.Generic;
using Factions;
using FishNet.Connection;
using Units;
using Units.Hero;

namespace Battle
{
    public struct Army
    {
        public NetworkConnection Connection;
        public Faction Faction;
        public HeroType HeroType;
        public AbilityType AbilityType;
        public Dictionary<UnitType, int> UnitCounts;
    }
}