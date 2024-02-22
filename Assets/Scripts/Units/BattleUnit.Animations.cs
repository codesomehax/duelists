using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(Animator))]
    public partial class BattleUnit
    {
        [Header("Animations")]
        [SerializeField] private AnimatorOverrideController animatorOverrideController;
        [SerializeField] private float idleAnimationSpeed = 1f;
        [SerializeField] private float runAnimationSpeed = 1f;
        [SerializeField] private float attackMeleeAnimationSpeed = 1f;
        [SerializeField] private float attackRangedAnimationSpeed = 1f;
        [SerializeField] private float hurtAnimationSpeed = 1f;
        [SerializeField] private float deathAnimationSpeed = 1f;

        private static readonly int IdleAnimationSpeedParameter = Animator.StringToHash("Idle Speed");
        private static readonly int RunAnimationSpeedParameter = Animator.StringToHash("Run Speed");
        private static readonly int AttackMeleeAnimationSpeedParameter = Animator.StringToHash("Attack Melee Speed");
        private static readonly int AttackRangedAnimationSpeedParameter = Animator.StringToHash("Attack Ranged Speed");
        private static readonly int HurtAnimationSpeedParameter = Animator.StringToHash("Hurt Speed");
        private static readonly int DeathAnimationSpeedParameter = Animator.StringToHash("Death Speed");

        private Animator _animator;
        
        private void SetupAnimator()
        {
            _animator.runtimeAnimatorController = animatorOverrideController;
            _animator.SetFloat(IdleAnimationSpeedParameter, idleAnimationSpeed);
            _animator.SetFloat(RunAnimationSpeedParameter, runAnimationSpeed);
            _animator.SetFloat(AttackMeleeAnimationSpeedParameter, attackMeleeAnimationSpeed);
            _animator.SetFloat(AttackRangedAnimationSpeedParameter, attackRangedAnimationSpeed);
            _animator.SetFloat(HurtAnimationSpeedParameter, hurtAnimationSpeed);
            _animator.SetFloat(DeathAnimationSpeedParameter, deathAnimationSpeed);
        }
    }
}