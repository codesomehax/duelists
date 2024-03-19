using System.Linq;
using UnityEngine;

namespace Units.Battle.Specialists.SpellCaster
{
    [CreateAssetMenu(fileName = "Red Energy Explosion Spell", menuName = "Duelists/Spells/Red Energy Explosion Spell", 
        order = 0)]
    public class RedEnergyExplosionSpell : Spell
    {
        [SerializeField] private RedEnergyExplosion redEnergyExplosionPrefab;
        
        public override void Cast(BattleUnit battleUnit)
        {
            BattleUnit[] enemyUnits = FindObjectsOfType<BattleUnit>()
                .Where(unit => unit.Owner != battleUnit.Owner)
                .ToArray();
            int damage = Mathf.CeilToInt(battleUnit.BaseDamage 
                                         * battleUnit.Count 
                                         * ((float)(100 + battleUnit.Intelligence * 10) / 100));
            
            foreach (BattleUnit enemyUnit in enemyUnits)
            {
                RedEnergyExplosion redEnergyExplosion = 
                    Instantiate(redEnergyExplosionPrefab, enemyUnit.transform.position, Quaternion.identity);
                redEnergyExplosion.OnHit += () => enemyUnit.TakeDamage(damage, AttackType.Magic);
                battleUnit.Spawn(redEnergyExplosion.NetworkObject, battleUnit.Owner);
            }
        }
    }
}