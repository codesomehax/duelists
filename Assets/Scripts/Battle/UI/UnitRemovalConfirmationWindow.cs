using System;
using UnityEngine;

namespace Battle.UI
{
    public class UnitRemovalConfirmationWindow : MonoBehaviour
    {
        public event Action OnRemovalConfirmed;
        
        public void ConfirmRemoval()
        {
            gameObject.SetActive(false);
            OnRemovalConfirmed?.Invoke();
        }

        public void CancelRemoval()
        {
            gameObject.SetActive(false);
        }
    }
}