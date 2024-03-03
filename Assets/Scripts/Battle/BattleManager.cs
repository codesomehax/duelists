using FishNet.Object;
using UnityEngine;

namespace Battle
{
    public class BattleManager : NetworkBehaviour
    {
        public override void OnStartClient()
        {
            Debug.Log("Hello on start client");
        }
    }
}