using FishNet;
using FishNet.Object;
using UnityEngine;

namespace Battle.UI
{
    public class InGameMenu : NetworkBehaviour
    {
        [SerializeField] private GameObject background;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                background.SetActive(!background.activeSelf);
        }

        public void ResumeGame()
        {
            background.SetActive(false);
        }

        public void QuitGame()
        {
            InstanceFinder.ClientManager.StopConnection();
            if (InstanceFinder.IsServer)
                InstanceFinder.ServerManager.StopConnection(true);
        }
    }
}