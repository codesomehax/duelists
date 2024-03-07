using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battle
{
    public class ActionTile3D : MonoBehaviour, IEquatable<ActionTile3D>
    {
        [SerializeField] private new string name;
        
        public bool Equals(ActionTile3D other)
        {
            return other != null && name == other.name;
        }
    }
}