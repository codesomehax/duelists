using System;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    public class UnitRemovalConfirmationWindow : MonoBehaviour
    {
        public event Action OnRemovalConfirmed;

        [SerializeField] private GameObject background;
        [SerializeField] private Button readyButton;
        
        public void ConfirmRemoval()
        {
            readyButton.interactable = true;
            background.SetActive(false);
            gameObject.SetActive(false);
            OnRemovalConfirmed?.Invoke();
        }

        public void CancelRemoval()
        {
            readyButton.interactable = false;
            background.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}