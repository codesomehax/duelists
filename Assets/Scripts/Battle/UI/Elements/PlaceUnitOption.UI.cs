using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battle.UI.Elements
{
    [RequireComponent(typeof(Image))]
    public partial class PlaceUnitOption : IPointerClickHandler
    {
        private static readonly Color NotSelectedColor = new(1, 1, 1, 0);
        private static readonly Color SelectedColor = new(1, 1, 1, 64f/255f);

        [Header("UI")] 
        [SerializeField] private TMP_Text unitNameText;
        [SerializeField] private TMP_Text unitCountText;

        private Image _image;

        private void SetUnitNameText(string unitName)
        {
            unitNameText.text = unitName;
        }

        private void SetUnitCountText(int count)
        {
            unitCountText.text = count.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSelected?.Invoke();
            Selected = true;
        }

        private void SetImageColor(bool selected)
        {
            _image.color = selected ? SelectedColor : NotSelectedColor;
        }
    }
}