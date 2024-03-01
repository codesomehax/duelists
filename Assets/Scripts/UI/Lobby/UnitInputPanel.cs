using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Units;
using UnityEngine;

namespace UI.Lobby
{
    public partial class UnitInputPanel : NetworkBehaviour
    {
        [SerializeField] private UnitType unitType;

        public event Action OnCountChangedServerside;

        [SyncVar] [NonSerialized] public int CostPerUnit;
        [SyncVar(OnChange = nameof(SyncUnitName))] [NonSerialized] public string UnitName;
        [SyncVar(OnChange = nameof(SyncCount))] [NonSerialized] private int _count;
        [SyncVar(OnChange = nameof(SyncMaxCount))] [NonSerialized] private int _maxCount;
        public int TotalCost => Count * CostPerUnit;
        
        public int Count
        {
            get => _count;
            private set
            {
                if (value >= 0 && value <= MaxCount)
                {
                    _count = value;
                    OnCountChangedServerside?.Invoke();
                }
            }
        }
        
        public int MaxCount
        {
            get => _maxCount;
            set
            {
                if (value < 0) return;
                Count = 0;
                _maxCount = value;
            }
        }
        
        public UnitType UnitType => unitType;

        public override void OnStartClient()
        {
            if (!IsOwner)
                slider.enabled = false;
        }

        [ServerRpc]
        private void SetCountServerRpc(int count)
        {
            Count = count;
        }
    }
}