using UnityEngine;

namespace Units
{
    public abstract class BattleGameObjectAnimator : MonoBehaviour
    {
        public abstract void Play(AnimationType animationType);
    }
}