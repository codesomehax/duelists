using Player;
using UnityEngine;

namespace UI.Lobby
{
    [RequireComponent(typeof(UI.Lobby.Lobby))]
    public class LobbyUIService : MonoBehaviour
    {
        [SerializeField] private Transform leftPlayerPanelContainer;
        [SerializeField] private Transform rightPlayerPanelContainer;

        private UI.Lobby.Lobby _lobby;

        private void Awake()
        {
            _lobby = GetComponent<UI.Lobby.Lobby>();
        }

        public Transform GetPlayerPanelContainerByPlayer(PlayerEnum player)
        {
            switch (player)
            {
                case PlayerEnum.Player1: return leftPlayerPanelContainer;
                case PlayerEnum.Player2: return rightPlayerPanelContainer;
                default: return null;
            }
        }
    }
}