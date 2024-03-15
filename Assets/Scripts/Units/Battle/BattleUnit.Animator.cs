using System;
using UnityEngine;

namespace Units.Battle
{
    [RequireComponent(typeof(Animator))]
    public partial class BattleUnit
    {
        private static readonly int RunningAnimatorParamHash = Animator.StringToHash("Running");
        private static readonly int AttackingMeleeAnimatorParamHash = Animator.StringToHash("Attacking Melee");
        private static readonly int AttackingRangedAnimatorParamHash = Animator.StringToHash("Attacking Ranged");
        private static readonly int HurtAnimatorParamHash = Animator.StringToHash("Hurt");
        private static readonly int DeadAnimatorParamHash = Animator.StringToHash("Dead");
        
        private Animator _animator;

        private void Animate(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.Idle:
                    _animator.SetBool(RunningAnimatorParamHash, false);
                    break;
                case AnimationType.Run:
                    _animator.SetBool(RunningAnimatorParamHash, true);
                    break;
                case AnimationType.AttackMelee:
                    _animator.SetTrigger(AttackingMeleeAnimatorParamHash);
                    break;
                case AnimationType.AttackRanged:
                    _animator.SetTrigger(AttackingRangedAnimatorParamHash);
                    break;
                case AnimationType.Hurt:
                    _animator.SetTrigger(HurtAnimatorParamHash);
                    break;
                case AnimationType.Death:
                    _animator.SetTrigger(DeadAnimatorParamHash);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
            }
        }
    }
}