using FishNet;
using Network.Lobby;
using TMPro;
using UnityEngine;

namespace UI.Menu
{
    [RequireComponent(typeof(LobbyNetworkService))]
    public class LanMenuManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private StartMenuManager startMenuManager;
        [SerializeField] private LobbyManager lobbyManager;
        
        private LobbyNetworkService _lobbyNetworkService;
        
        private void Awake()
        {
            _lobbyNetworkService = GetComponent<LobbyNetworkService>();
            InstanceFinder.ClientManager.OnAuthenticated += OnClientAuthenticated;
        }

        public void HostGame()
        {
            string username = usernameInputField.text;
            string password = passwordInputField.text;

            _lobbyNetworkService.HostGame(username, password);
        }

        public void JoinGame()
        {
            string username = usernameInputField.text;
            string ipAddress = ipAddressInputField.text;
            string password = passwordInputField.text;

            _lobbyNetworkService.JoinGame(username, ipAddress, password);
        }

        private void OnClientAuthenticated()
        {
            Debug.Log("Hello, I am authenticated");
            gameObject.SetActive(false);
            lobbyManager.gameObject.SetActive(true);
        }

        public void GoBackToStartMenu()
        {
            gameObject.SetActive(false);
            startMenuManager.gameObject.SetActive(true);
        }
    }
}