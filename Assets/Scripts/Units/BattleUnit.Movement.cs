using FishNet.Object;
using UnityEngine;

namespace Units
{
    public partial class BattleUnit
    {
        private static readonly Quaternion HostUnitsRotation = Quaternion.identity;
        private static readonly Quaternion ClientUnitsRotation = Quaternion.Euler(0, 180, 0);

        [Server]
        private void RotateTowardsEnemy()
        {
            transform.rotation = Owner.IsHost ? HostUnitsRotation : ClientUnitsRotation;
        }
    }
}