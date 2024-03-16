using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Units.Battle
{
    public partial class BattleUnit
    {
        private static readonly Color HostImageColor = new(140f / 255f, 22f / 255f, 22f / 255f);
        private static readonly Color ClientImageColor = new(22f / 255f, 22f / 255f, 140f / 255f);
        
        [Header("Canvas")]
        [SerializeField] private RectTransform canvasTransform;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text unitCountText;
        
        private Transform _cameraTransform;

        private void DisableClientCanvasOnHost()
        {
            if (!Owner.IsHost)
                canvasTransform.gameObject.SetActive(false);
        }

        private void ShowCanvas()
        {
            canvasTransform.gameObject.SetActive(true);
            bool isOwnerHost = Owner.IsHost || (!IsHost && Owner != LocalConnection);
            if (!isOwnerHost)
            {
                Vector3 localPosition = canvasTransform.localPosition;
                canvasTransform.localPosition = new Vector3(-localPosition.x, localPosition.y, localPosition.z);
            }
            image.color = isOwnerHost ? HostImageColor : ClientImageColor;
        }

        // TODO make the size of the image constant regardless of the distance to the camera
        private void RotateCanvasToCamera()
        {
            Vector3 canvasPosition = canvasTransform.position;
            Vector3 cameraPosition = _cameraTransform.position;
            Vector3 lookAtDirection = new(cameraPosition.x, cameraPosition.y, canvasPosition.z);
            canvasTransform.rotation = Quaternion.LookRotation(lookAtDirection - canvasPosition);
        }

        private void SyncUnitCount(int prev, int next, bool asServer)
        {
            if (asServer) return;
            Health = Count * SingleUnitHealth;
            unitCountText.text = next.ToString();
            UnitIcon.UnitCount = next;
        }
    }
}