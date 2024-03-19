﻿using UnityEngine;

namespace UI.Menu
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private GameObject lanMenu;

        public void Play()
        {
            gameObject.SetActive(false);
            lanMenu.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit(0);
        }
    }
}