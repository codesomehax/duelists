using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    public class PlayerUIComponents : MonoBehaviour
    {
        [SerializeField] private PlaceUnitWindow placeUnitWindow;
        [SerializeField] private UnitRemovalConfirmationWindow unitRemovalConfirmationWindow;
        [SerializeField] private Button readyButton;

        public PlaceUnitWindow PlaceUnitWindow => placeUnitWindow;
        public UnitRemovalConfirmationWindow UnitRemovalConfirmationWindow => unitRemovalConfirmationWindow;
        public Button ReadyButton => readyButton;
    }
}