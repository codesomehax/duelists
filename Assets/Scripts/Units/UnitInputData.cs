using Factions;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "UnitInputData", menuName = "Duelists/UnitInputData", order = 0)]
    public class UnitInputData : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private UnitType unitType;
        [SerializeField] private Faction faction;
        [SerializeField] private int defaultMaxCount;

        public string Name => name;
        public UnitType UnitType => unitType;
        public Faction Faction => faction;
        public int DefaultMaxCount => defaultMaxCount;
    }
}