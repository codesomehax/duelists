using System.Collections.Generic;
using FishNet;
using FishNet.Managing.Scened;
using UI.Lobby;
using UnityEngine;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        private void Awake()
        {
            InstanceFinder.SceneManager.OnLoadEnd += SceneManagerOnOnLoadEnd;
        }

        private void SceneManagerOnOnLoadEnd(SceneLoadEndEventArgs args)
        {
            if (!args.QueueData.AsServer) return;
            
            IEnumerable<ArmyData> armyDatas = args.QueueData.SceneLoadData.Params.ServerParams[0]
                as IEnumerable<ArmyData>;
            foreach (ArmyData armyData in armyDatas)
            {
                
            }
        }
    }
}