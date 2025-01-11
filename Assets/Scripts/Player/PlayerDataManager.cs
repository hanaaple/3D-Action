using System;
using System.ComponentModel;
using Character;
using Data;
using Data.Item.Base;
using Data.Item.Data;
using Data.Item.Scriptable;
using Data.Play;
using Data.ViewModel;
using Save;
using UI.Selectable.Slot;
using UnityEngine;
using Util;

namespace Player
{
    public enum ViewModelType
    {
        Status,
        Equip,
        CharacterData
    }
    
    public class PlayerDataManager : CharacterDataManager, ISavableObject
    {
        [Header("Equip Item")]
        [SerializeField] private ItemData<Weapon, WeaponStaticData> defaultWeapon;
        [SerializeField] private ItemData<Armor, ArmorStaticData> defaultHelmet;
        [SerializeField] private ItemData<Armor, ArmorStaticData> defaultBreastplate;
        [SerializeField] private ItemData<Armor, ArmorStaticData> defaultLeggings;
        [SerializeField] private ItemData<Armor, ArmorStaticData> defaultShoes;
        
        public StatusViewModel statusViewModel { get; private set; }
        public CharacterDataViewModel playerDataViewModel { get; private set; }
        public EquipViewModel equipViewModel { get; private set; }
        public OwnedItemViewModel ownedItemViewModel { get; private set; }

        
        // For Debug
        [SerializeField] private StatusData statusData;
        [SerializeField] private CharacterData characterData;
        [SerializeField] private EquippedItemData equipData;
        [SerializeField] private OwnedItemData ownedItemData;
        
        private void Awake()
        {
            Debug.LogWarning("Awake");
            
            
            // 맵 이동시에는?
            // 맵 이동과 Load를 구분할 필요가 있다.

            RegistSaveData();

            LoadOrCreateData();
        }

        private void Update()
        {
            if(!Application.isEditor) return;
            statusData = statusViewModel.statusData;
            characterData = playerDataViewModel.playerData;
            equipData = equipViewModel.equippedItemData;
            ownedItemData = ownedItemViewModel.ownedItemData;
        }

        public void RegistSaveData()
        {
            PlayDataManager.instance.RegistSavable(this);
        }

        public void LoadOrCreateData()
        {
            if (PlayDataManager.instance.TryLoad(out var saveData))
            {
                var data = saveData.GetSaveData<PlayerSaveData>(GetLabel(), GetId());

                LoadData(data);
            }
            else
            {
                CreateData();
            }
        }
        
        public void CreateData()
        {
            Debug.Log("CreateData");
            
            var statusData = new StatusData();
            var equippedItemData = new EquippedItemData(3, 4, 8);
            var playerData = new CharacterData();
            var ownedItemData = new OwnedItemData();
            ownedItemData.Initialize();
            
            statusData.level = 10;
            statusData.vitality = 10;
            statusData.spirit = 10;
            statusData.endurance = 10;
            statusData.strength = 10;

            InitializeViewModel(statusData, equippedItemData, ownedItemData, playerData);
            
            var instanceWeapon = ownedItemViewModel.AddItem(defaultWeapon.GetItem());
            
            var instanceHelmet = ownedItemViewModel.AddItem(defaultHelmet.GetItem());
            var instanceBreastplate = ownedItemViewModel.AddItem(defaultBreastplate.GetItem());
            var instanceLeggings = ownedItemViewModel.AddItem(defaultLeggings.GetItem());
            var instanceShoes = ownedItemViewModel.AddItem(defaultShoes.GetItem());

            equipViewModel.SetDefault(instanceWeapon, instanceHelmet, instanceBreastplate, instanceLeggings, instanceShoes);
            
            equipViewModel.SetArmor(instanceHelmet);
            equipViewModel.SetArmor(instanceBreastplate);
            equipViewModel.SetArmor(instanceLeggings);
            equipViewModel.SetArmor(instanceShoes);

            for (var i = 0; i < equipViewModel.GetMaxWeaponCount(); i++)
            {
                equipViewModel.SetWeapon(instanceWeapon, i, WeaponEquipType.Left);
                equipViewModel.SetWeapon(instanceWeapon, i, WeaponEquipType.Right);
            }

            playerDataViewModel.RecoveryAll();
        }

        private void LoadData(PlayerSaveData playerSaveData)
        {
            InitializeViewModel(playerSaveData.statusData, playerSaveData.equippedItemData, playerSaveData.ownedItemData, playerSaveData.playerData);
        }
        
        private void InitializeViewModel(StatusData statusData, EquippedItemData equippedItemData, OwnedItemData ownedItemData, CharacterData playerData)
        {
            // 다대다 관계이다. 많은 곳에서 이벤트를 발생시키고 많은 곳에서 이벤트를 받는다.
            
            // Event Handler에서 Event가 발생하도록 하지 말아라.
            // 이벤트 순환을 주의한다.
            
            // ViewModel이 여러번 생성되면, Null Observing이 늘어난다.
            
            statusViewModel = new StatusViewModel(statusData);
            equipViewModel = new EquipViewModel(equippedItemData);
            ownedItemViewModel = new OwnedItemViewModel(ownedItemData);
            playerDataViewModel = new CharacterDataViewModel(playerData, equipViewModel, statusViewModel);
        }

        // 다른 인스턴스에서 Update를 한 이후 ViewModel이 Initialize가 되는 것을 방지하기 위해 사용
        // 이를 위해 Initialize의 우선순위를 최상위로 두거나, 다른 방법을 사용하거나
        // private void BroadCastModelChange()
        // {
        //     playerDataViewModel.OnPropertyChanged();
        //     statusViewModel.OnPropertyChanged();
        //     equipViewModel.OnPropertyChanged();
        //     ownedItemViewModel.BroadCastStart();
        // }
        
        public string GetLabel()
        {
            return "Player";
        }

        public void AddPropertyChange(ViewModelType viewModelType, PropertyChangedEventHandler method)
        {
            // Dictionary로 작동X
            if (viewModelType == ViewModelType.Equip)
            {
                equipViewModel.PropertyChanged += method;
            }
            else if (viewModelType == ViewModelType.CharacterData)
            {
                playerDataViewModel.PropertyChanged += method;
            }
            else if (viewModelType == ViewModelType.Status)
            {
                statusViewModel.PropertyChanged += method;
            }
        }

        public void RemovePropertyChange(ViewModelType viewModelType, PropertyChangedEventHandler method)
        {
            // Dictionary로 작동X
            if (viewModelType == ViewModelType.Equip)
            {
                equipViewModel.PropertyChanged -= method;
            }
            else if (viewModelType == ViewModelType.CharacterData)
            {
                playerDataViewModel.PropertyChanged -= method;
            }
            else if (viewModelType == ViewModelType.Status)
            {
                statusViewModel.PropertyChanged -= method;
            }
        }

        public IObjectSaveData GetSaveData()
        {
            // 문제 발생. 각 여기서 들고있는 Data들은 ViewModel에 복사됨. 즉 ViewModel에서 직접 얻어와야됨.
            PlayerSaveData playerSaveData = new PlayerSaveData
            {
                statusData = statusViewModel.statusData,
                equippedItemData = equipViewModel.equippedItemData,
                ownedItemData = ownedItemViewModel.ownedItemData,
                playerData = playerDataViewModel.playerData
            };
            return playerSaveData;
        }

        // public CharacterData GetPlayerData()
        // {
        //     // 복사본임, 매번 복사하는 것은 생각보다 큰 비용이 들지도 모름.
        //     
        //     // 여기서 Event를 복사하면 안됨.
        //     return playerData;
        // }
    }
}