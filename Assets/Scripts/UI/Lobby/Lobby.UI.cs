using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public partial class Lobby
    {
        [SerializeField] private Button startGameButton;

        private void DisableStartGameButton()
        {
            startGameButton.enabled = false;
        }
    }
}