using System;
using System.Collections.Generic;
using CharacterControl;
using Data.Static;
using Interaction;
using Interaction.Base;
using Save;
using UnityEngine;
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

        private List<SavableInteraction> _savableInteractions = new();

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

            if (SaveManager.IsLoadEnable())
            {
                var saveData = SaveManager.Load();
                _player.LoadData(saveData);
            }
            else
            {
                _player.CreateData();
            }
        }

        private void Start()
        {
            if (SaveManager.IsLoadEnable())
            {
                var saveData = SaveManager.Load();
                
                foreach (var savableInteraction in _savableInteractions)
                {
                    savableInteraction.LoadData(saveData);
                }
            }

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
            var playerSaveData = _player.GetSaveData();
            
            SaveData saveData = new SaveData
            {
                playerSaveData = playerSaveData,
                InteractableSaveData = new Dictionary<string, InteractableSaveData>()
            };
            
            foreach (var savableInteraction in _savableInteractions)
            {
                saveData.InteractableSaveData.Add(savableInteraction.id, savableInteraction.GetSaveData());
            }
            
            SaveManager.Save(saveData);
        }

        public void RegistSavableInteraction(SavableInteraction savableInteraction)
        {
            _savableInteractions.Add(savableInteraction);
        }
    }
}