using System;
using Model;
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
    public class DataManager
    {
        private static DataManager _instance;

        public static DataManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataManager();
                }

                return _instance;
            }
        }

        public LevelUpTable LevelUpTable;
        
        // 다른 데이터도

        public StatusViewModel statusViewModel { get; private set; }
        public DescribeViewModel describeViewModel { get; private set; }
        public PlayerDataViewModel playerDataViewModel { get; private set; }
        public EquipViewModel equipViewModel { get; private set; }

        private DataManager()
        {
            StatusData statusData = new StatusData();
            EquippedItemData equippedItemData = new EquippedItemData();
            OwnedItemData ownedItemData = new OwnedItemData();
            PlayerData playerData = new PlayerData();

            statusViewModel = new StatusViewModel();
            statusViewModel.Initialize(statusData);

            describeViewModel = new DescribeViewModel();
            describeViewModel.Initialize();

            playerDataViewModel = new PlayerDataViewModel();
            playerDataViewModel.Initialize(equippedItemData, statusData);

            equipViewModel = new EquipViewModel();
            equipViewModel.Initialize(equippedItemData);
        }
    }
}