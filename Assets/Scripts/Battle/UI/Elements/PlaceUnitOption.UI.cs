using TMPro;
using UnityEngine;

namespace Battle.UI.Elements
{
    public partial class PlaceUnitOption
    {
        [Header("UI")] 
        [SerializeField] private TMP_Text unitNameText;
        [SerializeField] private TMP_Text unitCountText;

        public void SetUnitNameText(string unitName)
        {
            unitNameText.text = unitName;
        }

        public void SetUnitCountText(int count)
        {
            unitCountText.text = count.ToString();
        }
    }
}