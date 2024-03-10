using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public partial class BattleUnit
    {
        private static readonly Vector3 HostCanvasPosition = new(0.3f, 0.5f, -0.3f);
        private static readonly Vector3 ClientCanvasPosition = new(-0.3f, 0.5f, -0.3f);
        private static readonly Color HostImageColor = new(140f / 255f, 22f / 255f, 22f / 255f);
        private static readonly Color ClientImageColor = new(22f / 255f, 22f / 255f, 140f / 255f);
        
        [Header("Canvas")]
        [SerializeField] private RectTransform canvasTransform;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text unitCountText;
        
        private Transform _cameraTransform;

        private void AwakeCanvas()
        {
            canvasTransform.localPosition = IsHost ? HostCanvasPosition : ClientCanvasPosition;
            image.color = IsHost ? HostImageColor : ClientImageColor;
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
            unitCountText.text = next.ToString();
        }
    }
}