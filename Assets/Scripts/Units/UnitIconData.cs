using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "UnitIconData", menuName = "Duelists/Unit Icon Data", order = 0)]
    public class UnitIconData : ScriptableObject
    {
        [SerializeField] private UnitType unitType;
        [SerializeField] private Sprite icon;

        public UnitType UnitType => unitType;
        public Sprite Icon => icon;
    }
}