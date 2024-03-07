using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battle
{
    public class ActionTile3D : MonoBehaviour
    {
        public Color Color
        {
            get => _meshRenderer.material.color;
            set => _meshRenderer.material.color = value;
        }

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}