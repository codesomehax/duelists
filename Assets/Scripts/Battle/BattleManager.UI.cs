using System.Linq;
using FishNet.Object;
using Units;
using Units.Battle;
using UnityEngine;

namespace Battle
{
    public partial class BattleManager
    {
        [SerializeField] private RectTransform turnBarContent;

        [ObserversRpc]
        private void SetupIconsTransformObserversRpc()
        {
            foreach (BattleUnit battleUnit in FindObjectsOfType<BattleUnit>())
            {
                battleUnit.UnitIcon.gameObject.SetActive(true);
                Transform battleUnitTransform = battleUnit.UnitIcon.transform;
                battleUnitTransform.SetParent(turnBarContent);
                battleUnitTransform.rotation = Quaternion.identity;
            }   
        }

        [ObserversRpc]
        private void ArrangeUnitIconsObserversRpc(Vector3Int[] sortedCellPositions)
        {
            BattleUnit[] battleUnits = FindObjectsOfType<BattleUnit>();
            for (int i = 0; i < sortedCellPositions.Length; i++)
            {
                BattleUnit battleUnit = battleUnits.First(unit => unit.CellPosition == sortedCellPositions[i]);
                battleUnit.UnitIcon.transform.SetSiblingIndex(i);
            }
        }
    }
}