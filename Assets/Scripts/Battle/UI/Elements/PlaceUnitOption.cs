using System;
using Units;
using UnityEngine;

namespace Battle.UI.Elements
{
    public partial class PlaceUnitOption : MonoBehaviour
    {
        public static event Action<PlaceUnitOption> OnUnitTypeSelected;

        [SerializeField] private UnitType unitType;

        public UnitType UnitType => unitType;
    }
}