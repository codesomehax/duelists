using UnityEngine;

namespace Units.Battle
{
    [RequireComponent(typeof(BattleUnit))]
    public class Marksman : MonoBehaviour
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
            if (!_battleUnit.IsServer) return;
            
            Vector3 directionVector = _battleUnit.EnemyUnit.MovementTransform.position - projectileInHand.position;
            Quaternion rotation = Quaternion.LookRotation(directionVector);
            Projectile shotProjectile = Instantiate(projectilePrefab, projectileInHand.position, rotation);
            _battleUnit.Spawn(shotProjectile.NetworkObject, _battleUnit.Owner);
            shotProjectile.ShootAs(_battleUnit);
            
            if (deactivateProjectileAfterShooting)
                projectileInHand.gameObject.SetActive(false);
        }
    }
}