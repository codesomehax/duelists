using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI.Elements
{
    [RequireComponent(typeof(Image))]
    public class UnitIcon : MonoBehaviour
    {
        public Color Color { set => GetComponent<Image>().color = value; }
        public int UnitCount { set => unitCountText.text = value.ToString(); }

        [SerializeField] private TMP_Text unitCountText;
    }
}