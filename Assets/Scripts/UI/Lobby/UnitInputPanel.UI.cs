using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public partial class UnitInputPanel
    {
        [Header("UI")]
        [SerializeField] private TMP_Text unitNameText;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text countIndicator;

        private void SyncUnitName(string prev, string next, bool asServer)
        {
            if (asServer) return;
            unitNameText.text = next;
        }

        private void SyncCount(int prev, int next, bool asServer)
        {
            if (asServer) return;
            slider.value = next;
            countIndicator.text = $"{next} / {MaxCount}";
        }

        private void SyncMaxCount(int prev, int next, bool asServer)
        {
            if (asServer) return;
            slider.maxValue = next;
            countIndicator.text = $"{Count} / {next}";
        }

        public void PropagateSliderValueChange()
        {
            if (!IsOwner) return;
            SetCountServerRpc(Mathf.FloorToInt(slider.value));
        }
    }
}