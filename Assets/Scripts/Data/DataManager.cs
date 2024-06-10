using System;
using CharacterControl;
using Data.Static;
using UnityEngine;
using Util;
using ViewModel;

namespace Data
{
    /// <summary>
    /// 데이터를 관리
    /// ViewModel을 초기화
    /// </summary>
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