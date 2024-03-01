using TMPro;
using Units;
using UnityEngine;

namespace UI.Lobby.Settings
{
    public class UnitSettingPanel : MonoBehaviour
    {
        [SerializeField] private UnitInputData unitInputData;
        [SerializeField] private TMP_Text unitNameText;
        [SerializeField] private TMP_InputField unitMaxCountInputField;

        public UnitType UnitType => unitInputData.UnitType;
        public UnitSetting UnitSetting => new()
        {
            UnitName = unitInputData.Name,
            MaxCount = string.IsNullOrEmpty(unitMaxCountInputField.text)
                ? unitInputData.DefaultMaxCount
                : int.Parse(unitMaxCountInputField.text),
            Cost = unitInputData.Cost
        };
            
        private void Awake()
        {
            unitNameText.text = unitInputData.Name;
            unitMaxCountInputField.text = unitInputData.DefaultMaxCount.ToString();
        }
    }

    public struct UnitSetting
    {
        public string UnitName;
        public int MaxCount;
        public int Cost;
    }
}