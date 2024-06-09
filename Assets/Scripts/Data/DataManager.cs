using System;
using CharacterControl;
using Data.Static;
using UnityEngine;
using Util;
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

        private Player _player;

        public StatusViewModel playerStatusViewModel => _player.statusViewModel;
        public PlayerDataViewModel playerDataViewModel => _player.playerDataViewModel;
        public EquipViewModel playerEquipViewModel => _player.equipViewModel;
        public OwnedItemViewModel playerOwnedItemViewModel => _player.ownedItemViewModel;

        public SelectedUIViewModel selectedUiViewModel { get; private set; }

        public LevelUpTable LevelUpTable;

        // Add Item -> Item 중에서 선택해서 EquipData.Equip 인데

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else
            {
                Destroy(_instance);
            }

            _player = FindObjectOfType<Player>();

            LoadStaticData();

            selectedUiViewModel = new SelectedUIViewModel();
            selectedUiViewModel.Initialize();

            _player.LoadOrCreateData();
        }

        private void Start()
        {
            _player.BroadCastModelChange();
        }

        private void LoadStaticData()
        {
            // LevelUpTable = CsvReader.Load()
            // StatValueTable
            // DefaultPlayerDataTable
        }

        private void OnApplicationQuit()
        {
            var saveData = _player.GetSaveData();
            SaveManager.Save(saveData);
        }
    }
}