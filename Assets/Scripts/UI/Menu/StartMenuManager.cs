﻿using UnityEngine;

namespace UI.Menu
{
    public class StartMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject lanMenu;
        
        public void HotSeat()
        {
        }

        public void Lan()
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