using System.Collections;
using FishNet.Object;
using UnityEngine;

namespace Units.Battle
{
    public class Projectile : NetworkBehaviour
    {
        [SerializeField] private float speed;
        
        public void ShootAs(BattleUnit battleUnit)
        {
            StartCoroutine(ShootAsCoroutine(battleUnit));
        }

        private IEnumerator ShootAsCoroutine(BattleUnit battleUnit)
        {
            Vector3 target = battleUnit.EnemyUnit.MovementTransform.position;
            Vector3 Distance() => target - transform.position;
            bool ReachedTarget() => Distance().magnitude < 0.1f;

            Vector3 direction = Distance().normalized;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            while (!ReachedTarget())
            {
                transform.position += direction * (speed * Time.deltaTime);
                yield return null;
            }
            
            battleUnit.DealDamage();
            Despawn(NetworkObject);
        }
    }
}