using TMPro;
using UnityEngine;

namespace UI.Lobby
{
    public partial class GoldCountIndicator
    {
        [SerializeField] private TMP_Text goldCountText;

        private void SyncGoldCount(int prev, int next, bool asServer)
        {
            if (asServer) return;
            goldCountText.text = $"{AvailableGold} / {MaxGold}";
        }
    }
}