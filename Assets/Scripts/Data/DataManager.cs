using System;
using Model;
using UnityEngine;
using ViewModel;

namespace Data
{
    // 장착 중인 아이템
    // EquippedItemDataChange -> UpdatePlayView
    // EquippedItemDataChange -> EquipmentView
    // EquippedItemData -> PlayerDataChange -> UpdateStatusView
        
    // 스탯 변화
    // StatusDataChange -> PlayerDataChange -> UpdateStatusView
        
    // 계산되는 데이터
    // PlayerDataChange -> UpdatePlayerDataView
        
    // 보유 중인 장비
    // OwnedItemDataChange -> UpdateInventoryView
    
    
    [Serializable]
    public class DataManager : MonoBehaviour
    {
        private static DataManager _instance;

        public static DataManager instance => _instance;

        public LevelUpTable LevelUpTable;
        
        // 다른 데이터도

        public StatusViewModel statusViewModel { get; private set; }
        public DescribeViewModel describeViewModel { get; private set; }
        public PlayerDataViewModel playerDataViewModel { get; private set; }
        [field: SerializeField]
        public EquipViewModel equipViewModel { get; private set; }
        public OwnedItemViewModel ownedItemViewModel { get; private set; }
        
        private void Awake()
        {
            if(_instance == null)
                _instance = this;
            
            StatusData statusData = new StatusData();
            EquippedItemData equippedItemData = new EquippedItemData(4, 4, 4, 4, 4);
            OwnedItemData ownedItemData = new OwnedItemData();
            PlayerData playerData = new PlayerData();

            statusViewModel = new StatusViewModel();
            statusViewModel.Initialize(statusData);

            describeViewModel = new DescribeViewModel();
            describeViewModel.Initialize();

            playerDataViewModel = new PlayerDataViewModel();
            playerDataViewModel.Initialize(playerData, equippedItemData, statusData);

            equipViewModel ??= new EquipViewModel();
            equipViewModel.Initialize(equippedItemData);

            ownedItemViewModel = new OwnedItemViewModel();
            ownedItemViewModel.Initialize(ownedItemData);
        }
    }
}