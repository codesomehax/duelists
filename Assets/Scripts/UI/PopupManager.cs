using TMPro;
using UnityEngine;

namespace UI
{
    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private GameObject popupWindow;
        [SerializeField] private TMP_Text messageText;

        public void ShowPopupWithMessage(string message)
        {
            messageText.text = message;
            popupWindow.SetActive(true);
        }

        public void HidePopup()
        {
            popupWindow.SetActive(false);
        }
    }
}