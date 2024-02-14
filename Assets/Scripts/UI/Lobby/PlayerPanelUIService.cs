using TMPro;
using UnityEngine;

namespace UI.Lobby
{
    public class PlayerPanelUIService : MonoBehaviour
    {
        [SerializeField] private TMP_Text usernameText;

        public string Username { set => SetUsername(value); }

        private void SetUsername(string username)
        {
            usernameText.text = username;
        }
    }
}