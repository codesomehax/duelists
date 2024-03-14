using System;
using Battle.UI.Elements;
using UnityEngine;

namespace Units
{
    public partial class BattleUnit
    {
        [SerializeField] private UnitIcon unitIcon;

        public UnitIcon UnitIcon => unitIcon;

        private void SetupIcon()
        {
            bool isOwnerHost = Owner.IsHost || (!IsHost && Owner != LocalConnection);
            UnitIcon.Color = isOwnerHost ? HostImageColor : ClientImageColor;
        }
    }
}