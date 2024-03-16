using System;
using FishNet.Component.Animating;
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
        
        private NetworkAnimator _networkAnimator;

        private void Animate(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.Idle:
                    _networkAnimator.Animator.SetBool(RunningAnimatorParamHash, false);
                    break;
                case AnimationType.Run:
                    _networkAnimator.Animator.SetBool(RunningAnimatorParamHash, true);
                    break;
                case AnimationType.AttackMelee:
                    _networkAnimator.SetTrigger(AttackingMeleeAnimatorParamHash);
                    break;
                case AnimationType.AttackRanged:
                    _networkAnimator.SetTrigger(AttackingRangedAnimatorParamHash);
                    break;
                case AnimationType.Hurt:
                    _networkAnimator.SetTrigger(HurtAnimatorParamHash);
                    break;
                case AnimationType.Death:
                    _networkAnimator.Animator.SetBool(DeadAnimatorParamHash, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
            }
        }
    }
}