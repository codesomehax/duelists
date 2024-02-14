﻿using FishNet;
using Network;
using TMPro;
using UnityEngine;

namespace UI.Menu
{
    [RequireComponent(typeof(NetworkSetupManager))]
    public class LanMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private StartMenu startMenu;
        
        private NetworkSetupManager _networkSetupManager;
        
        private void Awake()
        {
            _networkSetupManager = GetComponent<NetworkSetupManager>();
            InstanceFinder.ClientManager.OnAuthenticated += OnClientAuthenticated;
        }

        public void HostGame()
        {
            string username = usernameInputField.text;
            string password = passwordInputField.text;

            _networkSetupManager.HostGame(username, password);
        }

        public void JoinGame()
        {
            string username = usernameInputField.text;
            string ipAddress = ipAddressInputField.text;
            string password = passwordInputField.text;

            _networkSetupManager.JoinGame(username, ipAddress, password);
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