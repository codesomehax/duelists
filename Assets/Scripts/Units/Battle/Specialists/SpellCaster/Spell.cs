using UnityEngine;

namespace Units.Battle.Specialists.SpellCaster
{
    public abstract class Spell : ScriptableObject
    {
        public abstract void Cast(BattleUnit battleUnit);
    }
}