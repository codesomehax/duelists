using System;
using UnityEngine;

namespace Units
{
    public partial class BattleUnit : MonoBehaviour
    {
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            SetupAnimator();
        }
    }
}