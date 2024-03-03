using System.Collections.Generic;
using Factions;
using FishNet.Object;
using Units;

namespace Battle
{
    public class PlayerManager : NetworkBehaviour
    {
        public Faction Faction { get; set; }
        public IDictionary<UnitType, int> UnitCounts { get; set; }
    }
}