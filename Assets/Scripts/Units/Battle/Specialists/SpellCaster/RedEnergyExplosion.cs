using System;
using FishNet.Object;
using UnityEngine;

namespace Units.Battle.Specialists.SpellCaster
{
    public class RedEnergyExplosion : NetworkBehaviour
    {
        private const float HitTimer = 1f;
        private const float FinishTimer = 2f;
        
        public event Action OnHit;

        private bool _eventEmitted;
        private bool HasFinished => _particleSystem.totalTime >= FinishTimer;

        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (HasFinished && IsServer)
            {
                Despawn();
                return;
            }
            
            if (Math.Abs(_particleSystem.time - HitTimer) < 0.1f && !_eventEmitted)
            {
                _eventEmitted = true;
                OnHit?.Invoke();
            }
        }
    }
}