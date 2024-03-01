using TMPro;
using UnityEngine;

namespace UI.Lobby.Settings
{
    public class GoldSettingPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField goldInputField;
        [SerializeField] private int defaultGoldCount;

        public int Gold => string.IsNullOrEmpty(goldInputField.text) 
            ? defaultGoldCount : int.Parse(goldInputField.text);

        private void Awake()
        {
            goldInputField.text = defaultGoldCount.ToString();
        }
    }
}