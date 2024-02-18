using TMPro;
using UnityEngine;

namespace UI.Lobby
{
    public partial class PlayerPanel
    {
        [Header("UI")]
        [SerializeField] private TMP_Text usernameText;

        private void SetUsernameText(string prev, string next, bool asServer)
        {
            if (asServer) return;
            usernameText.text = next;
        }
    }
}