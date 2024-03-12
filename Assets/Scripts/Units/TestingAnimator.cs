using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(Animator))]
    public class TestingAnimator : MonoBehaviour
    {
        private static readonly IDictionary<AnimationType, int> AnimationSpeedOverridesHashes;
        
        static TestingAnimator()
        {
            AnimationSpeedOverridesHashes = new Dictionary<AnimationType, int>(6);
            AnimationSpeedOverridesHashes[AnimationType.Idle] = Animator.StringToHash("Idle Speed");
            AnimationSpeedOverridesHashes[AnimationType.Run] = Animator.StringToHash("Run Speed");
            AnimationSpeedOverridesHashes[AnimationType.AttackMelee] = Animator.StringToHash("Attack Melee Speed");
            AnimationSpeedOverridesHashes[AnimationType.AttackRanged] = Animator.StringToHash("Attack Ranged Speed");
            AnimationSpeedOverridesHashes[AnimationType.Hurt] = Animator.StringToHash("Hurt Speed");
            AnimationSpeedOverridesHashes[AnimationType.Death] = Animator.StringToHash("Death Speed");
        }

        [SerializeField]
        [SerializedDictionary(keyName: "Animation type", valueName: "Animator")]
        private SerializedDictionary<AnimationType, Animator> customAnimators;
        
        [SerializeField]
        [SerializedDictionary(keyName: "Animation type", valueName: "Speed override")] 
        private SerializedDictionary<AnimationType, int> speedOverrides;

        [SerializeField] private AnimatorOverrideController animatorOverrideController;
        
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = animatorOverrideController;
            foreach (KeyValuePair<AnimationType, int> speedOverride in speedOverrides)
                _animator.SetFloat(AnimationSpeedOverridesHashes[speedOverride.Key], speedOverride.Value);
        }
    }
}