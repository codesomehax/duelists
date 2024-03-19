using FishNet.Object;
using UnityEngine;

namespace Units.Battle.Specialists.Marksman
{
    [RequireComponent(typeof(BattleUnit))]
    public class Marksman : NetworkBehaviour
    {
        [SerializeField] private Transform projectileInHand;
        [SerializeField] private bool deactivateProjectileAfterShooting;
        [SerializeField] private Projectile projectilePrefab;

        private BattleUnit _battleUnit;

        private void Awake()
        {
            _battleUnit = GetComponent<BattleUnit>();
        }

        private void AnimationEvent_ShowProjectile()
        {
            projectileInHand.gameObject.SetActive(true);
        }
        
        private void AnimationEvent_ProjectileShot()
        {
            if (!IsServer) return;
            
            Projectile shotProjectile = Instantiate(projectilePrefab, projectileInHand.position, projectileInHand.rotation);
            Spawn(shotProjectile.NetworkObject, Owner);
            shotProjectile.ShootAs(_battleUnit);
            
            if (deactivateProjectileAfterShooting)
                DeactivateProjectileObserversRpc();
        }

        [ObserversRpc]
        private void DeactivateProjectileObserversRpc()
        {
            projectileInHand.gameObject.SetActive(false);
        }
    }
}