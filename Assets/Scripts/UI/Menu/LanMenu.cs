using FishNet;
using Network;
using TMPro;
using UI.Lobby.Settings;
using UnityEngine;

namespace UI.Menu
{
    public class LanMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private StartMenu startMenu;
        [SerializeField] private LobbySettingsMenu lobbySettingsMenu;
        [SerializeField] private NetworkSetupManager networkSetupManager;
        
        private void Awake()
        {
            InstanceFinder.ClientManager.OnAuthenticated += OnClientAuthenticated;
        }

        public void GotoSettings()
        {
            gameObject.SetActive(false);
            lobbySettingsMenu.gameObject.SetActive(true);
        }

        public void HostGame()
        {
            string username = usernameInputField.text;
            string password = passwordInputField.text;

            networkSetupManager.HostGame(username, password);
        }

        public void JoinGame()
        {
            string username = usernameInputField.text;
            string ipAddress = ipAddressInputField.text;
            string password = passwordInputField.text;

            networkSetupManager.JoinGame(username, ipAddress, password);
        }

        private void OnClientAuthenticated()
        {
            Debug.Log("Hello, I am authenticated");
            gameObject.SetActive(false);
        }

        public void GoBackToStartMenu()
        {
            gameObject.SetActive(false);
            startMenu.gameObject.SetActive(true);
        }
    }
}