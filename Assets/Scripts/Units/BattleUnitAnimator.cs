using System;
using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(Animator))]
    public class BattleUnitAnimator : MonoBehaviour
    {
        private static readonly int IdleAnimationSpeedParameter = Animator.StringToHash("Idle Speed");
        private static readonly int RunAnimationSpeedParameter = Animator.StringToHash("Run Speed");
        private static readonly int AttackMeleeAnimationSpeedParameter = Animator.StringToHash("Attack Melee Speed");
        private static readonly int AttackRangedAnimationSpeedParameter = Animator.StringToHash("Attack Ranged Speed");
        private static readonly int HurtAnimationSpeedParameter = Animator.StringToHash("Hurt Speed");
        private static readonly int DeathAnimationSpeedParameter = Animator.StringToHash("Death Speed");
        
        [SerializeField] private AnimatorOverrideController animatorOverrideController;
        [SerializeField] private float idleAnimationSpeed = 1f;
        [SerializeField] private float runAnimationSpeed = 1f;
        [SerializeField] private float attackMeleeAnimationSpeed = 1f;
        [SerializeField] private float attackRangedAnimationSpeed = 1f;
        [SerializeField] private float hurtAnimationSpeed = 1f;
        [SerializeField] private float deathAnimationSpeed = 1f;

        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = animatorOverrideController;
            _animator.SetFloat(IdleAnimationSpeedParameter, idleAnimationSpeed);
            _animator.SetFloat(RunAnimationSpeedParameter, runAnimationSpeed);
            _animator.SetFloat(AttackMeleeAnimationSpeedParameter, attackMeleeAnimationSpeed);
            _animator.SetFloat(AttackRangedAnimationSpeedParameter, attackRangedAnimationSpeed);
            _animator.SetFloat(HurtAnimationSpeedParameter, hurtAnimationSpeed);
            _animator.SetFloat(DeathAnimationSpeedParameter, deathAnimationSpeed);
        }
        
        public void Play(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.Idle:
                    break;
                case AnimationType.Run:
                    break;
                case AnimationType.AttackMelee:
                    break;
                case AnimationType.AttackRanged:
                    break;
                case AnimationType.Hurt:
                    break;
                case AnimationType.Death:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
            }
        }
    }
}